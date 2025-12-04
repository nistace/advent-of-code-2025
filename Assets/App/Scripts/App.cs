using System;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Shared
{
   public class App : MonoBehaviour
   {
      [SerializeField] private MenuUi _menuUi;

      private void Start()
      {
         _menuUi.OnButtonClicked.AddListener(HandleMenuButtonClicked);
      }

      private async void HandleMenuButtonClicked((int day, int part) data)
      {
         await Solve(data.day, data.part);
      }

      private async UniTask Solve(int day, int part)
      {
         SharedUi.SetDayAndPart(day, part);
         _menuUi.gameObject.SetActive(false);

         var dayControllerType = GetTypeOfDayController(day);
         if (dayControllerType != null)
         {
            var dayController = (DayController)new GameObject().AddComponent(dayControllerType);
            await dayController.Solve(part, destroyCancellationToken);
            Destroy(dayController.gameObject);
            await UniTask.NextFrame(destroyCancellationToken);
         }

         _menuUi.gameObject.SetActive(true);
      }

      private static Type GetTypeOfDayController(int day)
      {
         return AppDomain.CurrentDomain.GetAssemblies().Reverse().Select(assembly => assembly.GetType($"Day{day:00}.DayController")).FirstOrDefault(type => type != null);
      }
   }
}