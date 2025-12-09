using System;
using System.Linq;
using UnityEngine;

namespace Day09
{
   public class DayController : Shared.DayController
   {
      protected override int DayNumber => 9;

      protected override string SolvePart1()
      {
         var allCoordinates = ReadInputLines().Select(t => t.Split(",")).Select(t => new Vector2Int(int.Parse(t[0]), int.Parse(t[1]))).ToArray();

         var biggestSurface = 0L;

         for (var i = 0; i < allCoordinates.Length; i++)
         {
            for (var j = i + 1; j < allCoordinates.Length; j++)
            {
               var first = allCoordinates[i];
               var second = allCoordinates[j];
               biggestSurface = Math.Max(biggestSurface, Math.Abs((1L + first.x - second.x) * (1L + first.y - second.y)));
            }
         }

         return $"{biggestSurface}";
      }

      protected override string SolvePart2()
      {
         return "";
      }
   }
}