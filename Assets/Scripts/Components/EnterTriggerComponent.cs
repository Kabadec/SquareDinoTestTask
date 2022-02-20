using Scripts.Utils;
using UnityEngine;

namespace Scripts.Components
{
    public class EnterTriggerComponent : MonoBehaviour
    {
        [SerializeField] private LayerMask _layer = ~0;
        [SerializeField] private EnterEvent _action;


        private void OnTriggerEnter(Collider other)
        {
            if (!other.gameObject.IsInLayer(_layer)) return;
            _action?.Invoke(other.gameObject);
        }
    }
}