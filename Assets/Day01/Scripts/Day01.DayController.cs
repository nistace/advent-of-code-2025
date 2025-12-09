using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace Day01
{
   public class DayController : Shared.DayController
   {
      protected override int DayNumber => 1;

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

      protected override UniTask<string> SimulatePart1(CancellationToken cancellationToken) => Simulate(false, cancellationToken);
      protected override UniTask<string> SimulatePart2(CancellationToken cancellationToken) => Simulate(true, cancellationToken);

      private async UniTask<string> Simulate(bool alwaysStopAtZero, CancellationToken cancellationToken)
      {
         var prefab = InstantiateDayPrefab(0).GetComponent<SimulationController>();
         var instructions = ReadInputLines().Select(t => t.Trim()).Select(t => (direction: t[0], steps: int.Parse(t[1..]))).ToArray();

         await UniTask.NextFrame();

         return await prefab.Simulate(instructions, alwaysStopAtZero, cancellationToken);
      }
   }
}