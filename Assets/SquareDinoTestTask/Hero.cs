using System;
using SquareDinoTestTask.Input;
using SquareDinoTestTask.Utils;
using SquareDinoTestTask.Utils.Disposables;
using SquareDinoTestTask.Utils.ObjectPool;
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
        [Header("Shooting")]
        [SerializeField] private GameObject _projectile;
        [SerializeField] private Vector3 _projectileSpawnOffset;
        [SerializeField] private float _shootDelay = 0.5f;

        public Vector3 MoveDest => _agent.destination;
        
        
        private Cooldown _cooldownShoot = new Cooldown();
        
        private readonly CompositeDisposable _trash = new CompositeDisposable();
        private static readonly int Velocity = Animator.StringToHash("velocity");


        private bool _trigger;
        
        private void Start()
        {
            _trash.Retain(InputManager.Instance.SubscribeOnLmbClick(OnLmbClick));
            _cooldownShoot.Value = _shootDelay;
        }

        private void OnLmbClick(Vector2 mousePos)
        {
            if(!_cooldownShoot.IsReady) return;
            
            var projectilePos = transform.position + _projectileSpawnOffset;
            var projectileDir = GetProjectileDir(mousePos, projectilePos);
            if(projectileDir == Vector3.zero) return;
            
            
            SpawnProjectile(projectilePos, projectileDir);
            _cooldownShoot.Reset();
        }

        private void SpawnProjectile(Vector3 pos, Vector3 dir)
        {
            var projectile = Pool.Instance.Get(_projectile, pos);
            var iProjectile = projectile.GetInterface<IProjectile>();
            if (iProjectile != null)
            {
                iProjectile.SetProjectile(dir.normalized);
            }
        }

        

        private Vector3 GetProjectileDir(Vector3 mousePos, Vector3 startProjectilePos)
        {
            var rayEndPoint = _mainCamera.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, 10f));
            var rayStartPoint = _mainCamera.transform.position;
            var rayDirection = rayEndPoint - rayStartPoint;
            var ray = new Ray(rayStartPoint, rayDirection);

            
            var normal = new Vector3(0f, 1f, 0f);
            var plane = new Plane(normal, startProjectilePos);

            float hitDist;
            if (!plane.Raycast(ray, out hitDist))
                return default;
            
            var pointOnPlane = ray.GetPoint(hitDist);
            var projectileDir = pointOnPlane - startProjectilePos;

            return projectileDir;
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
