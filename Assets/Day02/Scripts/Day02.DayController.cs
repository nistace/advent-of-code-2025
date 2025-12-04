using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using Shared;

namespace Day02
{
   public class DayController : Shared.DayController
   {
      protected override int DayNumber => 2;

      protected override async UniTask<string> SolvePart1(CancellationToken cancellationToken)
      {
         var ranges = ReadInputText().Split(",").Select(t => t.Trim()).Select(t => t.Split("-")).Select(t => (min: t[0], max: t[1])).ToArray();

         var invalidIDs = new HashSet<long>();

         foreach (var range in ranges)
         {
            var partToRepeat = range.min.Length == 1 ? 0 : long.Parse(range.min[..(range.min.Length / 2)]);
            var min = long.Parse(range.min);
            var max = long.Parse(range.max);

            var underMax = true;

            while (underMax)
            {
               var numberToCheck = long.Parse($"{partToRepeat}{partToRepeat}");

               underMax = numberToCheck <= max;
               if (underMax && numberToCheck >= min)
               {
                  invalidIDs.Add(numberToCheck);
               }

               partToRepeat++;
            }
         }

         return await UniTask.FromResult($"{invalidIDs.Sum()}");
      }

      protected override async UniTask<string> SolvePart2(CancellationToken cancellationToken)
      {
         var ranges = ReadInputText().Split(",").Select(t => t.Trim()).Select(t => t.Split("-")).Select(t => (min: t[0], max: t[1])).ToArray();

         var invalidIDs = new HashSet<long>();

         foreach (var range in ranges)
         {
            var min = long.Parse(range.min);
            var max = long.Parse(range.max);

            for (var number = min; number <= max; number++)
            {
               if (CheckNumberWithAnySequenceSize($"{number}"))
               {
                  invalidIDs.Add(number);
               }
            }
         }

         return await UniTask.FromResult($"{invalidIDs.Sum()}");
      }

      private static bool CheckNumberWithAnySequenceSize(string numberAsString)
      {
         if (numberAsString.Length < 2)
         {
            return false;
         }

         for (var sequenceSize = 1; sequenceSize <= numberAsString.Length / 2; sequenceSize++)
         {
            if (CheckNumberWithSequenceSize(numberAsString, sequenceSize))
            {
               return true;
            }
         }

         return false;
      }

      private static bool CheckNumberWithSequenceSize(string numberAsString, int sequenceSize)
      {
         if (numberAsString.Length % sequenceSize != 0)
         {
            return false;
         }

         for (var charIndex = 0; charIndex + sequenceSize < numberAsString.Length; charIndex++)
         {
            if (numberAsString[charIndex + sequenceSize] != numberAsString[charIndex])
            {
               return false;
            }
         }

         return true;
      }
   }
}