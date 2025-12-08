using System.Collections.Generic;
using System.Linq;

namespace Day02
{
   public class DayController : Shared.DayController
   {
      protected override int DayNumber => 2;

      protected override string SolvePart1()
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

         return $"{invalidIDs.Sum()}";
      }

      protected override string SolvePart2()
      {
         var ranges = ReadInputText().Split(",").Select(t => t.Trim()).Select(t => t.Split("-")).Select(t => (min: t[0], max: t[1])).ToArray();

         var sum = 0L;

         foreach (var range in ranges)
         {
            var min = long.Parse(range.min);
            var max = long.Parse(range.max);

            for (var number = min; number <= max; number++)
            {
               if (CheckNumberWithAnySequenceSize(number))
               {
                  sum += number;
               }
            }
         }

         return $"{sum}";
      }

      private static bool CheckNumberWithAnySequenceSize(long number)
      {
         var tryAgain = true;
         for (var modulo = 10L; tryAgain; modulo *= 10)
         {
            var sequence = number % modulo;
            if (sequence == 0)
            {
               continue;
            }

            var minValue = modulo * modulo / 10;
            var recomposedNumber = sequence * modulo + sequence;

            tryAgain = recomposedNumber < number;

            while (recomposedNumber < number)
            {
               minValue *= modulo;
               recomposedNumber = recomposedNumber * modulo + sequence;
            }

            if (number >= minValue && recomposedNumber == number)
            {
               return true;
            }
         }

         return false;
      }
   }
}