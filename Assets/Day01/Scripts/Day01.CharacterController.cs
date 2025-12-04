using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Day01
{
   public class CharacterController : MonoBehaviour
   {
      [SerializeField] private Transform _target;
      [SerializeField] private float _speed;

      private void Update()
      {
         if (_target == null) return;
         if (transform.position == _target.position) return;

         transform.forward = _target.position - transform.position;
         transform.position = Vector3.MoveTowards(transform.position, _target.position, _speed * Time.deltaTime);
      }

      public void SetTarget(Transform target, bool snap)
      {
         _target = target;

         if (snap)
         {
            transform.position = target.position;
         }
      }

      public async UniTask GoToTargetAsync(CancellationToken cancellationToken)
      {
         while (transform.position != _target.position)
         {
            await UniTask.NextFrame(cancellationToken);
         }
      }
   }
}