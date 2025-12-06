using System;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace Day06
{
   public class DayController : Shared.DayController
   {
      protected override int DayNumber => 6;

      protected override async UniTask<string> SolvePart1(CancellationToken cancellationToken)
      {
         var splitLines = ReadInputLines().Select(t => t.Split(' ', StringSplitOptions.RemoveEmptyEntries)).ToArray();
         var operators = splitLines[^1];
         Array.Resize(ref splitLines, splitLines.Length - 1);

         var operations = Enumerable.Range(0, splitLines[0].Length)
            .Select(index => (operands: splitLines.Select(t => long.Parse(t[index])).ToArray(), @operator: operators[index][0]))
            .ToArray();

         var sum = operations.Where(t => t.@operator == '+').Sum(t => t.operands.Sum()) +
                   operations.Where(t => t.@operator == '*').Sum(t => t.operands.Aggregate(1L, (current, other) => current * other));

         return await UniTask.FromResult($"{sum}");
      }

      protected override async UniTask<string> SolvePart2(CancellationToken cancellationToken)
      {
         var operandsLines = ReadInputLines().Select(t => t).ToArray();
         var operatorsLine = operandsLines[^1];
         Array.Resize(ref operandsLines, operandsLines.Length - 1);

         var sum = 0L;
         var index = 0;
         var multiplying = false;
         var operationBuffer = 0L;

         while (index < operatorsLine.Length)
         {
            if (operatorsLine[index] != ' ')
            {
               sum += operationBuffer;
               multiplying = operatorsLine[index] == '*';
               operationBuffer = multiplying ? 1 : 0;
            }

            if (long.TryParse(string.Join("", operandsLines.Select(t => t[index])).Trim(), out var operand))
            {
               if (multiplying)
               {
                  operationBuffer *= operand;
               }
               else
               {
                  sum += operand;
               }
            }

            index++;
         }

         sum += operationBuffer;

         return await UniTask.FromResult($"{sum}");
      }
   }
}