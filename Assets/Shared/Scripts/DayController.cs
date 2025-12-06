using System.Diagnostics;
using System.IO;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace Shared
{
   public abstract class DayController : MonoBehaviour
   {
      protected abstract int DayNumber { get; }
      private GameObject DayPrefab { get; set; }

      public string Solve(int part, out double totalMilliseconds)
      {
         var stopwatch = Stopwatch.StartNew();
         SharedUi.SetStopwatch(stopwatch);

         var result = (part switch
         {
            1 => SolvePart1(),
            2 => SolvePart2(),
            _ => default
         });

         stopwatch.Stop();
         totalMilliseconds = stopwatch.Elapsed.TotalMilliseconds;
         Debug.Log($"Solved Day {DayNumber} Part {part}: {result}\n{totalMilliseconds:0}ms");

         if (DayPrefab)
         {
            Destroy(DayPrefab);
         }

         return result;
      }

      public async UniTask<(string result, double ms)> Simulate(int part, CancellationToken cancellationToken)
      {
         var stopwatch = Stopwatch.StartNew();
         SharedUi.SetStopwatch(stopwatch);

         var result = await (part switch
         {
            1 => SimulatePart1(cancellationToken),
            2 => SimulatePart2(cancellationToken),
            _ => default
         });

         stopwatch.Stop();
         var ms = stopwatch.Elapsed.TotalMilliseconds;
         Debug.Log($"Simulated Day {DayNumber} Part {part}: {result}\n{ms:0}ms");

         if (DayPrefab)
         {
            Destroy(DayPrefab);
         }

         return (result, ms);
      }

      protected abstract string SolvePart1();
      protected abstract string SolvePart2();
      protected virtual UniTask<string> SimulatePart1(CancellationToken cancellationToken) => UniTask.FromResult("Not Implemented");
      protected virtual UniTask<string> SimulatePart2(CancellationToken cancellationToken) => UniTask.FromResult("Not Implemented");

      protected string ReadInputText() => File.ReadAllText($"{Application.streamingAssetsPath}/Day{DayNumber:00}.txt");
      protected string[] ReadInputLines() => File.ReadAllLines($"{Application.streamingAssetsPath}/Day{DayNumber:00}.txt");

      protected GameObject InstantiateDayPrefab(int part)
      {
         if (!DayPrefab)
         {
            DayPrefab = Instantiate(Resources.Load<GameObject>($"Day{DayNumber:00}.{part}"), transform);
         }

         return DayPrefab;
      }
   }
}