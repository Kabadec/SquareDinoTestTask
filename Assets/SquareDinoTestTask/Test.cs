using System;
using UnityEngine;

namespace SquareDinoTestTask
{
    public class Test : MonoBehaviour
    {
        [SerializeField] private float _startZ;
        [SerializeField] private float _endZ;
        [SerializeField] private float _speed;

        private void Update()
        {
            transform.position = new Vector3(transform.position.x, transform.position.y,
                transform.position.z + _speed * Time.deltaTime);

            var xpos = transform.position.z;
            if (xpos < _startZ || xpos > _endZ)
            {
                _speed *= -1;
            }
        }
    }
}