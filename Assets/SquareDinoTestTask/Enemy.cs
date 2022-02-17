using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace SquareDinoTestTask
{
    public class Enemy : MonoBehaviour, ISubject, ITakeDamage
    {
        [SerializeField] private float _hp = 2;
        [SerializeField] private UnityEvent _onDeath;
        
        private List<IObserver> _observers = new List<IObserver>();
        
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
            foreach (var observer in _observers)
            {
                observer.TakeNotify(this);
            }
        }

        public void TakeDamage(int damage)
        {
            _hp -= damage;
            
            if (_hp <= 0)
            {
                _hp = 0;
                _onDeath?.Invoke();
                
                Notify();
            }
        }
    }
}