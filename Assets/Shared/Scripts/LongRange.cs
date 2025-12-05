using System;
using System.Collections.Generic;
using System.Linq;

namespace Shared
{
   public readonly struct LongRange : IEquatable<LongRange>
   {
      private long Min { get; }
      private long Max { get; }

      public LongRange(long min, long max)
      {
         Min = min;
         Max = max;
      }

      // Check overlaps in the range, also returns true 
      public bool IsOverlappingOrAdjacent(LongRange other)
      {
         if (Contains(other.Min)) return true;
         if (Contains(other.Max)) return true;
         if (other.Contains(Min)) return true;
         if (other.Contains(Max)) return true;
         if (other.Min == Max + 1) return true;
         if (other.Max == Min - 1) return true;

         return false;
      }

      public bool Contains(long value) => value >= Min && value <= Max;

      public static LongRange MergeOverlappingRanges(List<LongRange> overlappingRanges) => new(overlappingRanges.Min(t => t.Min), overlappingRanges.Max(t => t.Max));

      public bool Equals(LongRange other) => Min == other.Min && Max == other.Max;
      public override bool Equals(object obj) => obj is LongRange other && Equals(other);
      public override int GetHashCode() => HashCode.Combine(Min, Max);
   }
}