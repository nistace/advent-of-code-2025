using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Shared
{
   public class MenuUi : MonoBehaviour
   {
      [SerializeField] private DayItemUi _buttonPrefab;
      [SerializeField] private Transform _buttonsContainer;
      [SerializeField] private Button _solveAllButton;

      public UnityEvent<(int day, int part)> OnSolveDayPartClicked { get; } = new();
      public UnityEvent<(int day, int part)> OnSimulateDayPartClicked { get; } = new();
      public UnityEvent OnSolveAllClicked => _solveAllButton.onClick;

      private void Start()
      {
         for (var day = 1; day <= 12; ++day)
         {
            var dayItemUi = Instantiate(_buttonPrefab, _buttonsContainer);
            dayItemUi.SetUp(day);

            var part1Data = (day, 1);
            dayItemUi.OnSolvePart1Clicked.AddListener(() => OnSolveDayPartClicked.Invoke(part1Data));
            dayItemUi.OnSimulatePart1Clicked.AddListener(() => OnSimulateDayPartClicked.Invoke(part1Data));

            var part2Data = (day, 2);
            dayItemUi.OnSolvePart2Clicked.AddListener(() => OnSolveDayPartClicked.Invoke(part2Data));
            dayItemUi.OnSimulatePart2Clicked.AddListener(() => OnSimulateDayPartClicked.Invoke(part2Data));
         }
      }
   }
}