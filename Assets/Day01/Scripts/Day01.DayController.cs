using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using Shared;

namespace Day01
{
   public class DayController : Shared.DayController
   {
      protected override int DayNumber => 1;

      protected override async UniTask<string> SolvePart1(CancellationToken cancellationToken) => await Run(false, cancellationToken);
      protected override async UniTask<string> SolvePart2(CancellationToken cancellationToken) => await Run(true, cancellationToken);

      private async UniTask<string> Run(bool alwaysStopAtZero, CancellationToken cancellationToken)
      {
         var prefab = InstantiateDayPrefab(0);
         await UniTask.NextFrame();
         var slotBuilder = prefab.GetComponentInChildren<SlotBuilder>();
         var characterController = prefab.GetComponentInChildren<CharacterController>();

         var instructions = ReadInputLines().Select(t => t.Trim()).Select(t => (direction: t[0], steps: int.Parse(t[1..]))).ToArray();
         slotBuilder.Initialize();
         characterController.SetTarget(slotBuilder.GetSlot(50), true);

         var currentValue = 50;
         var zeroes = 0;

         SharedUi.SetStep(0, instructions.Length);

         for (var instructionIndex = 0; instructionIndex < instructions.Length; instructionIndex++)
         {
            var instruction = instructions[instructionIndex];
            SharedUi.SetStep(instructionIndex, instructions.Length);

            var direction = instruction.direction == 'L' ? -1 : 1;

            for (var step = 0; step < instruction.steps; step++)
            {
               currentValue = (currentValue + 100 + direction) % 100;
               characterController.SetTarget(slotBuilder.GetSlot(currentValue), false);
               await characterController.GoToTargetAsync(cancellationToken);

               if (alwaysStopAtZero && currentValue == 0)
               {
                  zeroes++;
               }
            }

            if (!alwaysStopAtZero && currentValue == 0)
            {
               zeroes++;
            }
         }

         return $"{zeroes}";
      }
   }
}