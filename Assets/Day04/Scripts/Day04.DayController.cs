using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Day04
{
   public class DayController : Shared.DayController
   {
      protected override int DayNumber => 4;

      protected override async UniTask<string> SolvePart1(CancellationToken cancellationToken)
      {
         await UniTask.SwitchToMainThread();

         Vector2Int[] directions = { new(-1, -1), new(-1, 0), new(-1, 1), new(0, -1), new(0, 1), new(1, -1), new(1, 0), new(1, 1) };
         const int maxNeighbours = 3;

         var allRollsCoordinates = ReadInputLines()
            .SelectMany((line, y) => line.Select((c, x) => (value: c, coordinates: new Vector2Int(x, y))))
            .Where(t => t.value == '@')
            .Select(t => t.coordinates)
            .ToHashSet();

         var result = allRollsCoordinates.Count(cell => directions.Count(offset => allRollsCoordinates.Contains(cell + offset)) <= maxNeighbours);

         return await UniTask.FromResult($"{result}");
      }

      protected override async UniTask<string> SolvePart2(CancellationToken cancellationToken)
      {
         return await UniTask.FromResult("");
      }
   }
}