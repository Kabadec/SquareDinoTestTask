using System;
using Scripts.Utils;
using Scripts.Utils.Disposables;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Scripts.Input
{
    public class InputManager : Singleton<InputManager>
    {

        public delegate void TapTouchEvent(Vector2 position);
        public event TapTouchEvent OnTapTouch;

        public delegate void FirstLmbClickEvent(Vector2 position);

        public event FirstLmbClickEvent OnFirstLmbClick;

        private bool _isFirstLmbClick = true;
        
        private HeroInputActions _heroInput;
        
        public readonly Lock InputLocker = new Lock();


        private Vector2 TouchPosition => _heroInput.Hero.TouchPosition.ReadValue<Vector2>();


        private void Awake()
        {
            _heroInput = new HeroInputActions();
        }

        private void Start()
        {
            _heroInput.Hero.TouchTap.started += TouchTap;
        }
        
        public IDisposable SubscribeOnLmbClick(TapTouchEvent call)
        {
            OnTapTouch += call;
            return new ActionDisposable(() => OnTapTouch -= call);
        }
        
        public IDisposable SubscribeOnFirstLmbClick(FirstLmbClickEvent call)
        {
            OnFirstLmbClick += call;
            return new ActionDisposable(() => OnFirstLmbClick -= call);
        }
        
        private void TouchTap(InputAction.CallbackContext obj)
        {
            if(InputLocker.IsLocked)
                return;
            
            if (_isFirstLmbClick)
            {
                _isFirstLmbClick = false;
                OnFirstLmbClick?.Invoke(TouchPosition);
                return;
            }
            OnTapTouch?.Invoke(TouchPosition);
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
            _heroInput.Hero.TouchTap.started -= TouchTap;
        }
    }
}