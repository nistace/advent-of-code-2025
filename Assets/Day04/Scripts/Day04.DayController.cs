using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Day04
{
   public class DayController : Shared.DayController
   {
      protected override int DayNumber => 4;

      private readonly Vector2Int[] _directions = { new(-1, -1), new(-1, 0), new(-1, 1), new(0, -1), new(0, 1), new(1, -1), new(1, 0), new(1, 1) };
      private const int MAX_NEIGHBOURS = 3;

      private HashSet<Vector2Int> ReadInputCoordinatesHashSet() => ReadInputLines()
         .SelectMany((line, y) => line.Select((c, x) => (value: c, coordinates: new Vector2Int(x, y))))
         .Where(t => t.value == '@')
         .Select(t => t.coordinates)
         .ToHashSet();

      protected override async UniTask<string> SolvePart1(CancellationToken cancellationToken)
      {
         var allRollsCoordinates = ReadInputCoordinatesHashSet();
         var result = allRollsCoordinates.Count(cell => _directions.Count(offset => allRollsCoordinates.Contains(cell + offset)) <= MAX_NEIGHBOURS);

         return await UniTask.FromResult($"{result}");
      }

      protected override async UniTask<string> SolvePart2(CancellationToken cancellationToken)
      {
         var remainingRollsCoordinates = ReadInputCoordinatesHashSet();

         var removedInTotal = 0;
         int removedLastTime;

         do
         {
            removedLastTime = remainingRollsCoordinates.RemoveWhere(cell => _directions.Count(offset => remainingRollsCoordinates.Contains(cell + offset)) <= MAX_NEIGHBOURS);
            removedInTotal += removedLastTime;
         } while (removedLastTime > 0);

         return await UniTask.FromResult($"{removedInTotal}");
      }
   }
}