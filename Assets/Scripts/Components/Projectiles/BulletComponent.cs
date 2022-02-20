using Scripts.Utils.ObjectPool;
using UnityEngine;

namespace Scripts.Components.Projectiles
{
    [RequireComponent(typeof(PoolItem))]
    public class BulletComponent : MonoBehaviour, IProjectile
    {
        [SerializeField] private float _speed;
        [SerializeField] private float _lifeTime;

        private Vector3 _directionNorm = Vector3.zero;

        private float _startTime;

        private void Update()
        {
            if(Time.time - _startTime > _lifeTime)
                GetComponent<PoolItem>().Release();
            
            if(_directionNorm == Vector3.zero) return;

            transform.position += _directionNorm * _speed * Time.deltaTime;

        }

        public void SetProjectile(Vector3 direction)
        {
            _directionNorm = direction.normalized;
            transform.rotation = Quaternion.LookRotation(_directionNorm, Vector3.up);

            _startTime = Time.time;
        }
    }
}