using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Day01
{
   public class CharacterController : MonoBehaviour
   {
      private static readonly int MovingAnimParam = Animator.StringToHash("Moving");
      private static readonly int ScoreAnimParam = Animator.StringToHash("Score");

      [SerializeField] private float _speed;
      [SerializeField] private float _scoreDuration;
      [SerializeField] private Animator _animator;
      [SerializeField] private Transform _defaultTarget;
      [SerializeField] private TMP_Text _instructionText;
      [SerializeField] private Image _instructionProgressImage;

      public void SetPosition(Vector3 position)
      {
         transform.position = position;
      }

      public void SetInstructionProgress(string instruction, int currentStep, int totalSteps)
      {
         _instructionText.text = instruction;
         _instructionProgressImage.fillAmount = (float)currentStep / totalSteps;
      }

      public bool GoTowardsAndReturnTimeTaken(Vector3 position, float time, out float takenTime)
      {
         if (transform.position == position)
         {
            _animator.transform.forward = _defaultTarget.position - transform.position;
            _animator.SetBool(MovingAnimParam, false);
            takenTime = 0;
            return true;
         }

         _animator.transform.forward = position - transform.position;
         var distanceToTravel = (position - transform.position).magnitude;
         var requiredTime = distanceToTravel / _speed;

         if (requiredTime <= time)
         {
            transform.position = position;
            takenTime = requiredTime;
            _animator.SetBool(MovingAnimParam, false);

            return true;
         }

         transform.position = Vector3.Lerp(transform.position, position, time / requiredTime);
         takenTime = time;
         _animator.SetBool(MovingAnimParam, true);

         return false;
      }

      public void Score(out float takenTime)
      {
         _animator.SetTrigger(ScoreAnimParam);
         _animator.transform.forward = _defaultTarget.position - transform.position;
         takenTime = _scoreDuration;
      }
   }
}