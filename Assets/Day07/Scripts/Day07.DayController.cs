using System.Linq;

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
         const long splitterValue = -1L;
         var map = ReadInputLines()
            .Select(line => line.Select(character => character switch
               {
                  '^' => splitterValue,
                  'S' => 1,
                  _ => 0
               })
               .ToArray())
            .ToArray();

         var height = map.Length;
         var width = map[0].Length;

         for (var y = 1; y < height; ++y)
         {
            for (var x = 0; x < width; ++x)
            {
               if (map[y][x] == splitterValue && map[y - 1][x] > 0)
               {
                  map[y][x - 1] += map[y - 1][x];
                  map[y][x + 1] += map[y - 1][x];
               }
               else if (map[y - 1][x] > 0)
               {
                  map[y][x] += map[y - 1][x];
               }
            }
         }

         return $"{map[height - 1].Sum()}";
      }
   }
}