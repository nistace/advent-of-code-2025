using System.Linq;
using UnityEngine;

namespace Day07
{
   public class DayController : Shared.DayController
   {
      protected override int DayNumber => 7;

      protected override string SolvePart1()
      {
         var map = ReadInputLines().Select((line, _) => line.ToCharArray()).ToArray();
         var splits = 0;
         var height = map.Length;
         var width = map[0].Length;

         for (var y = 1; y < height; ++y)
         {
            for (var x = 1; x < width - 1; ++x)
            {
               if (map[y][x] == '^' && map[y - 1][x] == '|')
               {
                  splits++;
                  map[y][x - 1] = '|';
                  map[y][x + 1] = '|';
               }
               else if (map[y - 1][x] is '|' or 'S')
               {
                  map[y][x] = '|';
               }
            }
         }

         return $"{splits}";
      }

      protected override string SolvePart2()
      {
         return "";
      }
   }
}