using System;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace Day06
{
   public class DayController : Shared.DayController
   {
      protected override int DayNumber => 6;

      private void ReadInput(out (long[] operands, char @operator)[] operations)
      {
         var splitLines = ReadInputLines().Select(t => t.Split(' ', StringSplitOptions.RemoveEmptyEntries)).ToArray();
         var operators = splitLines[^1];
         Array.Resize(ref splitLines, splitLines.Length - 1);

         operations = Enumerable.Range(0, splitLines[0].Length)
            .Select(index => (operands: splitLines.Select(t => long.Parse(t[index])).ToArray(), @operator: operators[index][0]))
            .ToArray();
      }

      protected override async UniTask<string> SolvePart1(CancellationToken cancellationToken)
      {
         ReadInput(out var operations);

         var sum = operations.Where(t => t.@operator == '+').Sum(t => t.operands.Sum()) +
                   operations.Where(t => t.@operator == '*').Sum(t => t.operands.Aggregate(1L, (current, other) => current * other));

         return await UniTask.FromResult($"{sum}");
      }

      protected override async UniTask<string> SolvePart2(CancellationToken cancellationToken)
      {
         ReadInput(out _);
         return await UniTask.FromResult("");
      }
   }
}