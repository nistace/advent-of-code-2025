using System.Threading;
using Cysharp.Threading.Tasks;
using Shared;
using UnityEngine;

namespace Day03
{
   public class DayController : Shared.DayController
   {
      protected override int DayNumber => 3;

      protected override async UniTask<string> SolvePart1(CancellationToken cancellationToken)
      {
         var lines = ReadInputLines();

         var sum = 0;

         foreach (var line in lines)
         {
            var firstIndex = 0;
            for (var index = 1; index < line.Length - 1; index++)
            {
               if (line[index] > line[firstIndex])
               {
                  firstIndex = index;
               }
            }

            var secondIndex = firstIndex + 1;
            for (var index = secondIndex + 1; index < line.Length; index++)
            {
               if (line[index] > line[secondIndex])
               {
                  secondIndex = index;
               }
            }

            sum += int.Parse($"{line[firstIndex]}{line[secondIndex]}");
         }

         return await UniTask.FromResult($"{sum}");
      }

      protected override async UniTask<string> SolvePart2(CancellationToken cancellationToken)
      {
         const int batteriesCount = 12;
         var lines = ReadInputLines();
         var sum = 0L;

         foreach (var line in lines)
         {
            var digitAsString = "";
            var lastAddedIndex = -1;
            while (digitAsString.Length < batteriesCount)
            {
               lastAddedIndex++;
               var lastIndexToCheck = line.Length - (batteriesCount - digitAsString.Length);
               for (var index = lastAddedIndex + 1; index <= lastIndexToCheck; index++)
               {
                  if (line[index] > line[lastAddedIndex])
                  {
                     lastAddedIndex = index;
                  }
               }

               digitAsString += line[lastAddedIndex];
            }

            sum += long.Parse($"{digitAsString}");
         }

         return await UniTask.FromResult($"{sum}");
      }
   }
}