using System.Collections.Generic;
using Scripts.Components.Health;
using Scripts.Creatures;
using Scripts.Utils.Disposables;
using Scripts.Utils.Observer;
using UnityEngine;

namespace Scripts.Controllers
{
    public class AreaController : MonoBehaviour, ISubject
    {
        [SerializeField] private Transform _waypoint;
        [SerializeField] private GameObject _enemyContainer;
        [SerializeField] private Transform _cameraWaypoint;

        private List<HealthComponent> _enemies = new List<HealthComponent>();
        private List<IObserver> _observers = new List<IObserver>();
        private readonly CompositeDisposable _trash = new CompositeDisposable();

        private int _enemyCount = 0;

        
        private void Start()
        {
            _enemies = new List<HealthComponent>(_enemyContainer.GetComponentsInChildren<HealthComponent>());
            _enemyCount = _enemies.Count;
            foreach (var enemy in _enemies)
            {
                _trash.Retain(enemy._onDie.Subscribe(OnEnemyDied));
            }
        }

        private void OnEnemyDied()
        {
            _enemyCount--;
            if(_enemyCount == 0)
                Notify();
            else if(_enemyCount < 0)
                Debug.LogWarning("Enemy counter have a negative value!");
        }

        public void SelectThisArea(IMovable hero)
        {
            hero.MoveTo(_waypoint.position);
            CameraController.Instance.CameraMoveTo(_cameraWaypoint);
        }

        public void AddObserver(IObserver observer)
        {
            _observers.Add(observer);
        }

        public void RemoveObserver(IObserver observer)
        {
            _observers.Remove(observer);
        }

        public void Notify()
        {
            var observers = new List<IObserver>(_observers);
            foreach (var observer in observers)
            {
                observer.TakeNotify(this);
            }
        }

        private void OnDestroy()
        {
            _trash.Dispose();
        }
    }
}