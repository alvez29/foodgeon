using System;
using InputActions;
using Project.Code.Core;
using UnityEngine;
using UnityEngine.InputSystem;


namespace Project.Code.Gameplay.Player
{
    public class PlayerInputHandler : MonoBehaviour
    {
        #region Events
        
        public event Action OnDashPerformed;
        public event Action OnSimpleAbilityPerformed;
        public event Action OnSpecialAbilityPerformed;
        public event Action<Vector2> OnMoveInputChanged;
        public event Action<Vector2> OnMousePositionChanged;
        public event Action<Vector2> OnGamepadAimChanged;
        public event Action OnGamepadAimStopped;
        public event Action OnEatPerformed;
        public event Action OnZoomStarted;
        public event Action OnZoomStopped;
        
        
        #endregion

        #region Fields

        private PlayerControls _controls;
        
        #endregion
        
        #region Unity Functions

        private void Awake()
        {
            _controls = new PlayerControls();
            BindInputActions();
        }

        private void OnEnable() => EnableInput();
        private void OnDisable() => DisableInput();
        
        #endregion

        #region Private Methods

        private void OnAimPerformed(InputAction.CallbackContext ctx)
        {
            var input = ctx.ReadValue<Vector2>();
            
            if (ctx.control.device is not Mouse)
            {
                OnGamepadAimChanged?.Invoke(input);
            }
            else
            {
                OnMousePositionChanged?.Invoke(input);
            }
        }

        private void OnAimCanceled(InputAction.CallbackContext ctx)
        {
            OnGamepadAimStopped?.Invoke();
        }

        private void BindInputActions()
        {
            _controls.Player.Move.performed += OnMoveOnPerformed;
            _controls.Player.Move.canceled += OnMoveOnCanceled;

            _controls.Player.Aim.performed += OnAimPerformed;
            _controls.Player.Aim.canceled += OnAimCanceled;

            _controls.Player.Dash.performed += ctx => OnDashPerformed?.Invoke();
            _controls.Player.SimpleAttack.performed += ctx => OnSimpleAbilityPerformed?.Invoke();
            _controls.Player.SpecialAttack.performed += ctx => OnSpecialAbilityPerformed?.Invoke();
            
            _controls.Player.Eat.performed += ctx => OnEatPerformed?.Invoke();
            
            _controls.Player.ZoomOut.performed += ctx => OnZoomStarted?.Invoke();
            _controls.Player.ZoomOut.canceled += ctx => OnZoomStopped?.Invoke();
        }

        private void OnMoveOnPerformed(InputAction.CallbackContext ctx)
        {
            var moveInput = ctx.ReadValue<Vector2>();
            OnMoveInputChanged?.Invoke(moveInput);
        }

        private void OnMoveOnCanceled(InputAction.CallbackContext ctx)
        {
            OnMoveInputChanged?.Invoke(Vector2.zero);
        }


        public void DisableInput()
        {
            _controls?.Disable();
        }

        public void EnableInput()
        {
            _controls.Enable();
        }
        
        #endregion
    }
}