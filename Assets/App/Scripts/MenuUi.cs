using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Shared
{
   public class MenuUi : MonoBehaviour
   {
      [SerializeField] private GameObject _buttonPrefab;
      [SerializeField] private Transform _buttonsContainer;

      public UnityEvent<(int day, int part)> OnButtonClicked { get; } = new();

      private void Start()
      {
         for (var day = 1; day <= 12; ++day)
         {
            for (var part = 1; part <= 2; ++part)
            {
               var button = Instantiate(_buttonPrefab, _buttonsContainer);
               button.GetComponentInChildren<TMP_Text>().text = $"Day {day} Part {part}";

               var eventData = (day, part);
               button.GetComponentInChildren<Button>().onClick.AddListener(() => OnButtonClicked.Invoke(eventData));
            }
         }
      }
   }
}