using System;
using SquareDinoTestTask.Input;
using SquareDinoTestTask.Utils;
using SquareDinoTestTask.Utils.Disposables;
using UnityEngine;
using UnityEngine.AI;

namespace SquareDinoTestTask
{
    public class Hero : MonoBehaviour, IMovable
    {
        [SerializeField] private Camera _mainCamera;
        [SerializeField] private NavMeshAgent _agent;
        [SerializeField] private Animator _animator;
        [SerializeField] private float _velocityToLockInput = 0.1f;
        
        private readonly CompositeDisposable _trash = new CompositeDisposable();
        private static readonly int Velocity = Animator.StringToHash("velocity");


        private bool _trigger;
        
        private void Start()
        {
            _trash.Retain(InputManager.Instance.SubscribeOnLmbClick(OnLmbClick));
        }

        private void OnLmbClick(Vector2 position)
        {
            if (Physics.Raycast(_mainCamera.ScreenPointToRay(position), out var hit))
            {
                _agent.SetDestination(hit.point);
            }
        }

        private void Update()
        {
            var agentVelocity = _agent.velocity.magnitude;
            _animator.SetFloat(Velocity, agentVelocity);

            if (_trigger && agentVelocity > _velocityToLockInput)
            {
                _trigger = false;
                InputManager.Instance.InputLocker.Retain(this);
            }
            else if (!_trigger && agentVelocity <= _velocityToLockInput)
            {
                _trigger = true;
                InputManager.Instance.InputLocker.Release(this);
            }
        }

        public void MoveTo(Vector3 dest)
        {
            _agent.SetDestination(dest);
        }
        
        public void OnDestroy()
        {
            _trash.Dispose();
        }

        
    }
}
