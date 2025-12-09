using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Shared;
using UnityEngine;

namespace Day01
{
   public class SimulationController : MonoBehaviour
   {
      [SerializeField] private SlotBuilder _slotBuilder;
      [SerializeField] private CharacterController _characterController;

      public async UniTask<string> Simulate((char direction, int steps)[] instructions, bool alwaysStopAtZero, CancellationToken cancellationToken)
      {
         var currentPosition = 50;
         var zeroes = 0;

         _slotBuilder.Initialize();
         _characterController.SetPosition(_slotBuilder.GetSlot(currentPosition).position);

         SharedUi.SetStep(0, instructions.Length);
         SharedUi.SetOutput($"{zeroes}");

         await UniTask.NextFrame(cancellationToken);

         var frameRemainingTime = Time.deltaTime;

         for (var instructionIndex = 0; instructionIndex < instructions.Length; instructionIndex++)
         {
            var instruction = instructions[instructionIndex];
            var instructionLabel = instruction.direction + "" + instruction.steps;
            var direction = instruction.direction == 'L' ? -1 : 1;
            float takenTime;

            SharedUi.SetStep(instructionIndex, instructions.Length);

            for (var step = 0; step < instruction.steps; step++)
            {
               currentPosition = (currentPosition + 100 + direction) % 100;

               _characterController.SetInstructionProgress(instructionLabel, step, instruction.steps);
               while (!_characterController.GoTowardsAndReturnTimeTaken(_slotBuilder.GetSlot(currentPosition).position, frameRemainingTime, out takenTime))
               {
                  frameRemainingTime = await EvaluateFrameRemainingTimeAsync(frameRemainingTime, takenTime, cancellationToken);
               }

               if (alwaysStopAtZero && currentPosition == 0)
               {
                  zeroes++;
                  _characterController.SetInstructionProgress("Score!", step, instruction.steps);
                  _characterController.Score(out takenTime);
                  frameRemainingTime = await EvaluateFrameRemainingTimeAsync(frameRemainingTime, takenTime, cancellationToken);
                  SharedUi.SetOutput($"{zeroes}");
               }
            }

            if (!alwaysStopAtZero && currentPosition == 0)
            {
               zeroes++;
               _characterController.SetInstructionProgress("Score!", instruction.steps, instruction.steps);
               _characterController.Score(out takenTime);
               frameRemainingTime = await EvaluateFrameRemainingTimeAsync(frameRemainingTime, takenTime, cancellationToken);
               SharedUi.SetOutput($"{zeroes}");
            }
         }

         return $"{zeroes}";
      }

      private static async Task<float> EvaluateFrameRemainingTimeAsync(float thisFrameTime, float requiredTime, CancellationToken cancellationToken)
      {
         if (thisFrameTime > requiredTime)
         {
            return thisFrameTime - requiredTime;
         }

         var waitedTime = thisFrameTime;
         while (waitedTime - requiredTime <= float.Epsilon)
         {
            await UniTask.NextFrame(PlayerLoopTiming.Update, cancellationToken);
            waitedTime += Time.deltaTime;
         }

         return waitedTime - requiredTime;
      }
   }
}