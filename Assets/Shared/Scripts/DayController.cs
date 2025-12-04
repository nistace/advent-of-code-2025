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

      public async UniTask<string> Solve(int part, CancellationToken cancellationToken)
      {
         var stopwatch = Stopwatch.StartNew();
         SharedUi.SetStopwatch(stopwatch);

         var result = await (part switch
         {
            1 => SolvePart1(cancellationToken),
            2 => SolvePart2(cancellationToken),
            _ => default
         });

         stopwatch.Stop();
         Debug.Log($"Day {DayNumber} Part {part}: {result}\n{stopwatch.Elapsed.TotalMilliseconds:0}ms");

         if (DayPrefab)
         {
            Destroy(DayPrefab);
         }

         return result;
      }

      protected abstract UniTask<string> SolvePart1(CancellationToken cancellationToken);
      protected abstract UniTask<string> SolvePart2(CancellationToken cancellationToken);

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