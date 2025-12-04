using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Day01
{
   public class SlotBuilder : MonoBehaviour
   {
      private const int SLOTS = 100;

      [SerializeField] private float _radius = 3;
      [SerializeField] private GameObject _slotPrefab;
      [SerializeField] private GameObject _zeroSlotPrefab;

      private List<Transform> Slots { get; } = new();

      public Transform GetSlot(int number) => Slots[number % SLOTS];

      public void Initialize()
      {
         foreach (var slot in Slots)
         {
            Destroy(slot);
         }

         Slots.Clear();
         
         for (var i = 0; i < SLOTS; ++i)
         {
            var position = Quaternion.Euler(0, 360f * i / SLOTS, 0) * new Vector3(0, 0, _radius);

            var slot = Instantiate(i == 0 ? _zeroSlotPrefab : _slotPrefab, position, Quaternion.Euler(0, 180 + 360 * i / 100f, 0), transform);
            slot.GetComponentInChildren<TMP_Text>().text = $"{i}";
            Slots.Add(slot.transform);
         }
      }
   }
}