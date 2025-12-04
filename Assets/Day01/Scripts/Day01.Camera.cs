using UnityEngine;

namespace Day01
{
   public class Camera : MonoBehaviour
   {
      [SerializeField] private Transform _target;
      [SerializeField] private float _smooth = .5f;

      private Vector3 _currentVelocity;

      private void Update()
      {
         transform.forward = Vector3.SmoothDamp(transform.forward, _target.position - transform.position, ref _currentVelocity, _smooth);
      }
   }
}