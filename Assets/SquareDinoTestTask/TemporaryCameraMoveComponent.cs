using System;
using UnityEngine;

namespace SquareDinoTestTask
{
    public class TemporaryCameraMoveComponent : MonoBehaviour
    {
        [SerializeField] private Transform _target;
        [SerializeField] private Vector3 _offset;


        private void Update()
        {
            transform.position = _target.position + _offset;
        }
    }
}