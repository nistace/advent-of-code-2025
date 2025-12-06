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
         _menuUi.OnSolveAllClicked.AddListener(HandleSolveAllClicked);
         _menuUi.OnSolveDayPartClicked.AddListener(HandleSolveButtonClicked);
         _menuUi.OnSimulateDayPartClicked.AddListener(HandleSimulateButtonClicked);
      }

      private async void HandleSolveAllClicked() => await SolveAll();

      private async void HandleSimulateButtonClicked((int day, int part) data)
      {
         await Simulate(data.day, data.part);
      }

      private async void HandleSolveButtonClicked((int day, int part) data) => await Solve(data.day, data.part);

      private async UniTask<double> SolveAll()
      {
         var totalMs = 0D;
         for (var day = 1; day <= AppSettings.DAYS; ++day)
         {
            totalMs += (await Solve(day, 1)).ms;
            totalMs += (await Solve(day, 2)).ms;
         }

         Debug.Log($"Solved All parts in {totalMs:0}ms");
         return totalMs;
      }

      private async UniTask<(string result, double ms)> Solve(int day, int part)
      {
         SharedUi.SetDayAndPart(day, part);
         _menuUi.gameObject.SetActive(false);

         var result = string.Empty;
         var ms = 0D;

         var dayControllerType = GetTypeOfDayController(day);
         if (dayControllerType != null)
         {
            var dayController = (DayController)new GameObject().AddComponent(dayControllerType);
            result = dayController.Solve(part, out ms);
            Destroy(dayController.gameObject);
            await UniTask.NextFrame(destroyCancellationToken);
         }

         _menuUi.gameObject.SetActive(true);

         return (result, ms);
      }

      private async UniTask<(string result, double ms)> Simulate(int day, int part)
      {
         SharedUi.SetDayAndPart(day, part);
         _menuUi.gameObject.SetActive(false);

         var result = string.Empty;
         var ms = 0D;

         var dayControllerType = GetTypeOfDayController(day);
         if (dayControllerType != null)
         {
            var dayController = (DayController)new GameObject().AddComponent(dayControllerType);
            (result, ms) = await dayController.Simulate(part, destroyCancellationToken);
            Destroy(dayController.gameObject);
            await UniTask.NextFrame(destroyCancellationToken);
         }

         _menuUi.gameObject.SetActive(true);

         return (result, ms);
      }

      private static Type GetTypeOfDayController(int day)
      {
         return AppDomain.CurrentDomain.GetAssemblies().Reverse().Select(assembly => assembly.GetType($"Day{day:00}.DayController")).FirstOrDefault(type => type != null);
      }
   }
}