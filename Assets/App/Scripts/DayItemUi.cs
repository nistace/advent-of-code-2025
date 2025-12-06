using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Shared
{
   public class DayItemUi : MonoBehaviour
   {
      [SerializeField] private TMP_Text _dayText;
      [SerializeField] private Button _solvePart1Button;
      [SerializeField] private Button _solvePart2Button;
      [SerializeField] private Button _simulatePart1Button;
      [SerializeField] private Button _simulatePart2Button;

      public UnityEvent OnSolvePart1Clicked => _solvePart1Button.onClick;
      public UnityEvent OnSolvePart2Clicked => _solvePart2Button.onClick;
      public UnityEvent OnSimulatePart1Clicked => _simulatePart1Button.onClick;
      public UnityEvent OnSimulatePart2Clicked => _simulatePart2Button.onClick;

      public void SetUp(int day) => _dayText.text = $"Day {day}";
   }
}