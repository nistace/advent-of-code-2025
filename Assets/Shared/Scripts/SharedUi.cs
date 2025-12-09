using System;
using System.Diagnostics;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
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
      [SerializeField] private GameObject[] _simulationObjects;
      [SerializeField] private MsFiller _realTimeMsFiller;
      [SerializeField] private MsFiller _simulationTimeMsFiller;
      [SerializeField] private Output _output;
      [SerializeField] private SimulationSpeed _simulationSpeed;

      private Stopwatch _stopwatch;
      private float _simulationStart = -1;

      private void Awake()
      {
         Instance = this;
         _output.OnClick.AddListener(_output.CopyToClipboard);
         _simulationSpeed.OnValueChanged.AddListener(HandleSliderValueChanged);
      }

      private void HandleSliderValueChanged(float newValue) => _simulationSpeed.SetWithRatioValue(newValue);
      public static void SetTimeScale(float timeScale) => Instance._simulationSpeed.SetWithTimeScale(timeScale);

      public static void SetDayAndPart(int day, int part)
      {
         Instance._dayText.text = $"Day {day}";
         Instance._partText.text = $"Part {part}";
      }

      public static void SetStep(int currentStep, int numberOfSteps)
      {
         Instance._stepText.text = $"Step {Mathf.Clamp(currentStep + 1, 1, numberOfSteps)} / {numberOfSteps}";
         Instance._progressFiller.fillAmount = (float)currentStep / numberOfSteps;
      }

      public static void SetStopwatch(Stopwatch stopwatch) => Instance._stopwatch = stopwatch;
      public static void StartSimulation() => Instance._simulationStart = Time.time;
      public static void StopSimulation() => Instance._simulationStart = -1;

      public static void SetOutput(string output) => Instance._output.Value = output;

      private void Update()
      {
         if (_stopwatch != null) _realTimeMsFiller.RefreshRealTime(_stopwatch);
         if (_simulationStart >= 0) _simulationTimeMsFiller.RefreshSimulationTime(_simulationStart);
      }

      public static void SetSimulation(bool simulation)
      {
         foreach (var simulationGameObject in Instance._simulationObjects)
         {
            simulationGameObject.SetActive(simulation);
         }
      }

      [Serializable]
      private class MsFiller
      {
         [SerializeField] private TMP_Text _text;
         [SerializeField] private Image _filler;

         public void RefreshRealTime(Stopwatch stopwatch)
         {
            _filler.fillAmount = stopwatch.Elapsed.Milliseconds / 1000f;
            _text.text = $"{stopwatch.Elapsed.TotalSeconds:0.000}";
         }

         public void RefreshSimulationTime(float startTime)
         {
            _filler.fillAmount = (Time.time - startTime) % 1;
            _text.text = $"{Time.time - startTime:0.000}";
         }
      }

      [Serializable]
      private class Output
      {
         [SerializeField] private TMP_Text _outputText;
         [SerializeField] private Button _copyOutputButton;

         public string Value
         {
            get => _outputText.text;
            set => _outputText.text = value;
         }

         public UnityEvent OnClick => _copyOutputButton.onClick;

         public void CopyToClipboard() => GUIUtility.systemCopyBuffer = _outputText.text;
      }

      [Serializable]
      private class SimulationSpeed
      {
         [SerializeField] private Slider _slider;
         [SerializeField] private TMP_Text _text;
         [SerializeField] private float _valueCoefficient = 100;
         [SerializeField] private float _valuePower = 3.825f;
         [SerializeField] private float _sliderStep = .1f;

         public UnityEvent<float> OnValueChanged => _slider.onValueChanged;

         public void SetWithTimeScale(float timeScale) => SetWithRatioValue(Mathf.Pow(timeScale / _valueCoefficient, 1 / _valuePower));

         public void SetWithRatioValue(float newValue)
         {
            var steppedValue = Mathf.RoundToInt(newValue / _sliderStep) * _sliderStep;
            _slider.SetValueWithoutNotify(steppedValue);
            Time.timeScale = _valueCoefficient * Mathf.Pow(steppedValue, _valuePower);
            _text.SetText($"Simulation Speed: {Time.timeScale:0.0}");
         }
      }
   }
}