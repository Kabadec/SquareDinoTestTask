using Scripts.Components.Health;
using Scripts.Controllers;
using Scripts.Utils.Disposables;
using UnityEngine;

namespace Scripts.UI
{
    public class LifeBarWidget : MonoBehaviour
    {
        [SerializeField] private ProgressBarWidget _lifeBar;
        [SerializeField] private HealthComponent _hp;

        private readonly CompositeDisposable _trash = new CompositeDisposable();
        private int _maxHp;


        
        private void Start()
        {
            if (_hp == null)
                _hp = GetComponentInParent<HealthComponent>();

            _maxHp = _hp.Health;

            _trash.Retain(_hp._onDie.Subscribe(OnDie));
            _trash.Retain(_hp._onChange.Subscribe(OnHpChanged));
            
        }

        private void Update()
        {
            transform.LookAt(CameraController.Instance.MainCamera.transform, Vector3.up);
        }

        private void OnDie()
        {
            gameObject.SetActive(false);
        }

        private void OnHpChanged(int hp)
        {
            var progress = (float) hp / _maxHp;
            if (_lifeBar != null)
                _lifeBar.SetProgress(progress);
        }

        private void OnDestroy()
        {
            _trash.Dispose();
        }
    }
}