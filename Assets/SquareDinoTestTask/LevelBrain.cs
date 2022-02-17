using System;
using SquareDinoTestTask.Input;
using SquareDinoTestTask.Utils.Disposables;
using UnityEngine;

namespace SquareDinoTestTask
{
    public class LevelBrain : MonoBehaviour, IObserver
    {
        [SerializeField] private AreaBrain[] _areas;

        private int _pointer = 0;
        
        private readonly CompositeDisposable _trash = new CompositeDisposable();


        private void Start()
        {
            _trash.Retain(InputManager.Instance.SubscribeOnFirstLmbClick(OnFirstLmbClick));
        }

        private void OnFirstLmbClick(Vector2 position)
        {
            NextArea();
        }


        private void NextArea()
        {
            _areas[_pointer].RemoveObserver(this);
            _pointer++;
            _areas[_pointer].AddObserver(this);
            _areas[_pointer].SelectThisArea();
        }

        public void TakeNotify(ISubject subject)
        {
            NextArea();
        }

        private void OnDestroy()
        {
            _trash.Dispose();
        }
    }
}