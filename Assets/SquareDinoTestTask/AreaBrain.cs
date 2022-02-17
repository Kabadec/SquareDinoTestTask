﻿using System;
using System.Collections.Generic;
using UnityEngine;

namespace SquareDinoTestTask
{
    public class AreaBrain : MonoBehaviour, IObserver, ISubject
    {
        [SerializeField] private Transform _waypoint;
        [SerializeField] private GameObject _enemyContainer;
        [SerializeField] private Vector3 _cameraWaypoint;

        private List<ISubject> _enemies = new List<ISubject>();
        private List<IObserver> _observers = new List<IObserver>();

        
        private void Start()
        {
            _enemies = new List<ISubject>(_enemyContainer.GetComponentsInChildren<ISubject>());
            foreach (var subject in _enemies)
            {
                subject.AddObserver(this);
            }
        }

        public void SelectThisArea()
        {
            
        }

        public void TakeNotify(ISubject subject)
        {
            _enemies.Remove(subject);
            
            if(_enemies.Count == 0)
                Notify();
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
            foreach (var observer in _observers)
            {
                observer.TakeNotify(this);
            }
        }
    }
}