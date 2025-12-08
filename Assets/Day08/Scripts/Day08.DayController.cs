using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Day08
{
   public class DayController : Shared.DayController
   {
      protected override int DayNumber => 8;

      protected override string SolvePart1()
      {
         const int requiredConnections = 1000;
         const int largestCircuitsToTake = 3;

         var allCoordinates = ReadInputLines()
            .Select(t => t.Split(','))
            .Select(t => new Vector3Int(int.Parse(t[0]), int.Parse(t[1]), int.Parse(t[2])))
            .OrderBy(t => t.x)
            .ThenBy(t => t.y)
            .ThenBy(t => t.z)
            .ToArray();

         var distances = allCoordinates.SelectMany((first, index) =>
               allCoordinates.Skip(index + 1).Select(second => (from: first, to: second, sqrMagnitude: Vector3.SqrMagnitude(first - second))))
            .OrderBy(t => t.sqrMagnitude)
            .ToArray();

         var circuitIndicesPerNode = new Dictionary<Vector3Int, int>();
         var circuits = new List<HashSet<Vector3Int>>();

         var connections = 0;

         for (var distanceIndex = 0; distanceIndex < distances.Length && connections < requiredConnections; ++distanceIndex)
         {
            var workConnection = distances[distanceIndex];

            var fromIsConnected = circuitIndicesPerNode.TryGetValue(workConnection.from, out var fromCircuit);
            var toIsConnected = circuitIndicesPerNode.TryGetValue(workConnection.to, out var toCircuit);

            if (fromIsConnected && toIsConnected && fromCircuit == toCircuit)
            {
               // Both are already connected together
            }
            else if (fromIsConnected && toIsConnected)
            {
               // Both are part of distinct circuits => we merge them
               foreach (var toCircuitItem in circuits[toCircuit])
               {
                  circuits[fromCircuit].Add(toCircuitItem);
                  circuitIndicesPerNode[toCircuitItem] = fromCircuit;
               }

               circuits[toCircuit].Clear();
            }
            else if (fromIsConnected)
            {
               circuits[fromCircuit].Add(workConnection.to);
               circuitIndicesPerNode[workConnection.to] = fromCircuit;
            }
            else if (toIsConnected)
            {
               circuits[toCircuit].Add(workConnection.from);
               circuitIndicesPerNode[workConnection.from] = toCircuit;
            }
            else
            {
               var newCircuitIndex = circuits.Count;
               circuits.Add(new HashSet<Vector3Int> { workConnection.from, workConnection.to });
               circuitIndicesPerNode[workConnection.from] = newCircuitIndex;
               circuitIndicesPerNode[workConnection.to] = newCircuitIndex;
            }

            connections++;
         }

         var result = circuits.Select(t => t.Count).Where(t => t > 0).OrderByDescending(t => t).Take(largestCircuitsToTake).Aggregate(1, (t, u) => t * u);

         return $"{result}";
      }

      protected override string SolvePart2()
      {
         var allCoordinates = ReadInputLines()
            .Select(t => t.Split(','))
            .Select(t => new Vector3Int(int.Parse(t[0]), int.Parse(t[1]), int.Parse(t[2])))
            .OrderBy(t => t.x)
            .ThenBy(t => t.y)
            .ThenBy(t => t.z)
            .ToArray();

         var nodeCount = allCoordinates.Length;

         var distances = allCoordinates.SelectMany((first, index) =>
               allCoordinates.Skip(index + 1).Select(second => (from: first, to: second, sqrMagnitude: Vector3.SqrMagnitude(first - second))))
            .OrderBy(t => t.sqrMagnitude)
            .ToArray();

         var circuitIndicesPerNode = new Dictionary<Vector3Int, int>();
         var circuits = new List<HashSet<Vector3Int>>();
         var lastModifiedCircuitSize = 0;
         (Vector3Int from, Vector3Int to, float sqrMagnitude) lastConnection = default;

         for (var distanceIndex = 0; distanceIndex < distances.Length && lastModifiedCircuitSize < nodeCount; ++distanceIndex)
         {
            lastConnection = distances[distanceIndex];

            var fromIsConnected = circuitIndicesPerNode.TryGetValue(lastConnection.from, out var fromCircuit);
            var toIsConnected = circuitIndicesPerNode.TryGetValue(lastConnection.to, out var toCircuit);

            if (fromIsConnected && toIsConnected && fromCircuit == toCircuit)
            {
               // Both are already connected together
            }
            else if (fromIsConnected && toIsConnected)
            {
               // Both are part of distinct circuits => we merge them
               foreach (var toCircuitItem in circuits[toCircuit])
               {
                  circuits[fromCircuit].Add(toCircuitItem);
                  circuitIndicesPerNode[toCircuitItem] = fromCircuit;
               }

               circuits[toCircuit].Clear();
               lastModifiedCircuitSize = circuits[fromCircuit].Count;
            }
            else if (fromIsConnected)
            {
               circuits[fromCircuit].Add(lastConnection.to);
               circuitIndicesPerNode[lastConnection.to] = fromCircuit;
               lastModifiedCircuitSize = circuits[fromCircuit].Count;
            }
            else if (toIsConnected)
            {
               circuits[toCircuit].Add(lastConnection.from);
               circuitIndicesPerNode[lastConnection.from] = toCircuit;
               lastModifiedCircuitSize = circuits[toCircuit].Count;
            }
            else
            {
               var newCircuitIndex = circuits.Count;
               circuits.Add(new HashSet<Vector3Int> { lastConnection.from, lastConnection.to });
               circuitIndicesPerNode[lastConnection.from] = newCircuitIndex;
               circuitIndicesPerNode[lastConnection.to] = newCircuitIndex;
               lastModifiedCircuitSize = 2;
            }
         }

         var result = lastConnection.from.x * lastConnection.to.x;

         return $"{result}";
      }
   }
}