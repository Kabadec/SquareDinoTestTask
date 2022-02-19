using System;
using SquareDinoTestTask.Utils;
using SquareDinoTestTask.Utils.ObjectPool;
using UnityEngine;

namespace SquareDinoTestTask
{
    [RequireComponent(typeof(PoolItem))]
    public class BulletComponent : MonoBehaviour, IProjectile
    {
        [SerializeField] private int _damage;
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

        public void DealDamage(GameObject target)
        {
            var damageTaker = target.GetInterface<ITakeDamage>();
            if (damageTaker == null) return;
            
            damageTaker.TakeDamage(_damage);
            
            //Debug.Log("Damage dealed");
        }
    }
}