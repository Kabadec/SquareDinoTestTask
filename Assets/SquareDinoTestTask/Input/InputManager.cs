using System;
using SquareDinoTestTask.Utils;
using SquareDinoTestTask.Utils.Disposables;
using UnityEngine;
using UnityEngine.InputSystem;

namespace SquareDinoTestTask.Input
{
    public class InputManager : Singleton<InputManager>
    {

        public delegate void LmbClickEvent(Vector2 position);

        public event LmbClickEvent OnLmbClick;
        
        public delegate void FirstLmbClickEvent(Vector2 position);

        public event FirstLmbClickEvent OnFirstLmbClick;

        private bool _isFirstLmbClick = true;
        
        private HeroInputActions _heroInput;
        
        public readonly Lock InputLocker = new Lock();


        private Vector2 MousePosition => _heroInput.Hero.MousePosition.ReadValue<Vector2>();


        private void Awake()
        {
            _heroInput = new HeroInputActions();
        }

        private void Start()
        {
            _heroInput.Hero.LmbClick.canceled += LmbCanceled;
        }
        
        public IDisposable SubscribeOnLmbClick(LmbClickEvent call)
        {
            OnLmbClick += call;
            return new ActionDisposable(() => OnLmbClick -= call);
        }
        
        public IDisposable SubscribeOnFirstLmbClick(FirstLmbClickEvent call)
        {
            OnFirstLmbClick += call;
            return new ActionDisposable(() => OnFirstLmbClick -= call);
        }
        
        private void LmbCanceled(InputAction.CallbackContext obj)
        {
            if(InputLocker.IsLocked)
                return;
            
            if (_isFirstLmbClick)
            {
                _isFirstLmbClick = false;
                OnFirstLmbClick?.Invoke(MousePosition);
                return;
            }
            OnLmbClick?.Invoke(MousePosition);
        }
        
        
        
        
        private void OnEnable()
        {
            _heroInput.Enable();
        }
        private void OnDisable()
        {
            _heroInput.Disable();
        }

        private void OnDestroy()
        {
            _heroInput.Hero.LmbClick.canceled -= LmbCanceled;
        }
    }
}