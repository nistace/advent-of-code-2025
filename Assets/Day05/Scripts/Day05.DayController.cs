using System.Collections.Generic;
using System.Linq;
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

      protected override string SolvePart1()
      {
         ReadInput(out var freshRanges, out var ingredients);

         var result = ingredients.Count(ingredient => freshRanges.Any(range => range.Contains(ingredient)));

         return $"{result}";
      }

      protected override string SolvePart2()
      {
         ReadInput(out var freshRanges, out _);

         var result = freshRanges.Sum(t => t.Size);

         return $"{result}";
      }
   }
}