using UnityEngine;

namespace Scripts.Components.Health
{
    public class ModifyHealthComponent : MonoBehaviour
    {
        [SerializeField] private int _hpDelta;
        
        public void ModifyHealth(GameObject go)
        {
            var healthComponent = go.GetComponentInParent<HealthComponent>();

            if (healthComponent != null)
            {
                healthComponent.ModifyHealth(_hpDelta);
            }
            
        }
        
        public void SetHpDelta(int hpDelta)
        {
            _hpDelta = hpDelta;
        }
    }
    
}