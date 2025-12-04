using System.Diagnostics;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Shared
{
   public class SharedUi : MonoBehaviour
   {
      private static SharedUi Instance { get; set; }

      [SerializeField] private TMP_Text _stepText;
      [SerializeField] private Image _progressFiller;
      [SerializeField] private TMP_Text _dayText;
      [SerializeField] private TMP_Text _partText;
      [SerializeField] private TMP_Text _msText;
      [SerializeField] private Image _msFiller;

      private Stopwatch _stopwatch;

      
      private void Awake()
      {
         Instance = this;
      }

      public static void SetDayAndPart(int day, int part)
      {
         Instance._dayText.text = $"Day {day}";
         Instance._partText.text = $"Part {part}";
      }

      public static void SetStopwatch(Stopwatch stopwatch)
      {
         Instance._stopwatch = stopwatch;
      }

      public static void SetStep(int currentStep, int numberOfSteps)
      {
         Instance._stepText.text = $"Step {Mathf.Clamp(currentStep + 1, 1, numberOfSteps)} / {numberOfSteps}";
         Instance._progressFiller.fillAmount = (float)currentStep / numberOfSteps;
      }

      private void Update()
      {
         if (_stopwatch != null)
         {
            _msFiller.fillAmount = _stopwatch.Elapsed.Milliseconds / 1000f;
            _msText.text = $"{_stopwatch.Elapsed.TotalSeconds:0.000}";
         }
      }
   }
}