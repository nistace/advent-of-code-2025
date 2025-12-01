using System;
using UnityEngine;

namespace Shared
{
   public abstract class Day : MonoBehaviour
   {
      private enum Part
      {
         First = 0,
         Second = 1
      }

      [SerializeField] private Part _part;
      [SerializeField] private TextAsset _input;

      private void Start()
      {
         var result = (_part switch
         {
            Part.First => (Func<string>)SolvePart1,
            Part.Second => SolvePart2,
            _ => throw new ArgumentOutOfRangeException()
         })();

         Debug.Log($"{_part} Part: {result}");
      }

      protected abstract string SolvePart1();
      protected abstract string SolvePart2();

      protected string ReadInputText() => _input.text;
      protected string[] ReadInputLines() => _input.text.Split('\n');
   }
}