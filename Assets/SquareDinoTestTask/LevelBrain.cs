using System;
using SquareDinoTestTask.Input;
using SquareDinoTestTask.Utils;
using SquareDinoTestTask.Utils.Disposables;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SquareDinoTestTask
{
    public class LevelBrain : MonoBehaviour, IObserver
    {
        [SerializeField] private Hero _hero;
        [SerializeField] private AreaBrain[] _areas;

        private int _pointer = 0;
        
        private readonly CompositeDisposable _trash = new CompositeDisposable();

        private IMovable _heroMove;

        private bool _isLastArea = false;

        private void Start()
        {
            _trash.Retain(InputManager.Instance.SubscribeOnFirstLmbClick(OnFirstLmbClick));
            _heroMove = _hero.gameObject.GetInterface<IMovable>();
        }

        private void OnFirstLmbClick(Vector2 position)
        {
            NextArea(_heroMove);
            
        }

        private void Update()
        {
            if(!_isLastArea) return;
            
            if(!(Vector3.Distance(_hero.transform.position, _hero.MoveDest) < 0.2f)) return;
            
            var scene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(scene.name);
        }


        private void NextArea(IMovable heroMove)
        {
            _areas[_pointer].RemoveObserver(this);
            _pointer++;
            _areas[_pointer].AddObserver(this);
            _areas[_pointer].SelectThisArea(heroMove);
            
            
            if (_pointer == _areas.Length - 1)
            {
                _isLastArea = true;
            }
        }

        public void TakeNotify(ISubject subject)
        {
            NextArea(_heroMove);
        }

        private void OnDestroy()
        {
            _trash.Dispose();
        }
    }
}