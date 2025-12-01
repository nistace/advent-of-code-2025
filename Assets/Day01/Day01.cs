using System.Linq;
using Shared;

public class Day01 : Day
{
   protected override string SolvePart1()
   {
      var instructions = ReadInputLines().Select(t => t.Trim()).Select(t => (direction: t[0], steps: int.Parse(t[1..]))).ToArray();

      var value = 50;
      var zeroes = 0;

      foreach (var instruction in instructions)
      {
         value += (instruction.direction == 'L' ? -1 : 1) * instruction.steps;
         value = (value % 100 + 100) % 100;

         if (value == 0)
         {
            zeroes++;
         }
      }

      return $"{zeroes}";
   }

   protected override string SolvePart2()
   {
      var instructions = ReadInputLines().Select(t => t.Trim()).Select(t => (direction: t[0], steps: int.Parse(t[1..]))).ToArray();

      var value = 50;
      var zeroes = 0;

      foreach (var instruction in instructions)
      {
         value += (instruction.direction == 'L' ? -1 : 1) * instruction.steps;
         while (value > 99)
         {
            value -= 100;
            zeroes++;
         }

         while (value < 0)
         {
            value += 100;
            zeroes++;
         }
      }

      return $"{zeroes}";
   }
}