using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using Shared;

namespace Day05
{
   public class DayController : Shared.DayController
   {
      protected override int DayNumber => 5;

      private void ReadInput(out HashSet<LongRange> freshRanges, out long[] values)
      {
         var inputLines = ReadInputLines();
         freshRanges = new HashSet<LongRange>();

         var inputFreshRanges = inputLines.TakeWhile(t => !string.IsNullOrEmpty(t))
            .Select(t => t.Split('-'))
            .Select(t => new LongRange(long.Parse(t[0]), long.Parse(t[1])))
            .ToArray();

         foreach (var nextRange in inputFreshRanges)
         {
            var overlappingRanges = freshRanges.Where(nextRange.IsOverlappingOrAdjacent).ToList();
            freshRanges.ExceptWith(overlappingRanges);
            overlappingRanges.Add(nextRange);
            freshRanges.Add(LongRange.MergeOverlappingRanges(overlappingRanges));
         }

         values = inputLines.Skip(inputFreshRanges.Length + 1).ToArray().Select(long.Parse).ToArray();
      }

      protected override async UniTask<string> SolvePart1(CancellationToken cancellationToken)
      {
         ReadInput(out var freshRanges, out var ingredients);

         var result = ingredients.Count(ingredient => freshRanges.Any(range => range.Contains(ingredient)));

         return await UniTask.FromResult($"{result}");
      }

      protected override async UniTask<string> SolvePart2(CancellationToken cancellationToken)
      {
         ReadInput(out var freshRanges, out _);

         var result = freshRanges.Sum(t => t.Size);

         return await UniTask.FromResult($"{result}");
      }
   }
}