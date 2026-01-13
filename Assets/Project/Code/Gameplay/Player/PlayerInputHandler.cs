using System;
using InputActions;
using Project.Code.Core;
using UnityEngine;
using UnityEngine.InputSystem;


namespace Project.Code.Gameplay.Player
{
    
    public class PlayerInputHandler : MonoBehaviour
    {
        private const string GamepadScheme = "Gamepad Control Scheme";
        private const string MouseAndKeyboardScheme = "Mouse & Keyboard Control Scheme";
        
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

        public event Action OnInputDisabled;
        public event Action OnInputEnabled;
        #endregion

        #region Fields

        private PlayerControls _controls;
        private string _currentScheme;
        
        #endregion
        
        #region Unity Functions

        private void Awake()
        {
            _controls = new PlayerControls();
            BindInputActions(_controls);
            _controls.asset.bindingMask = null;
        }

        private void OnEnable() => EnableInput();
        private void OnDisable() => DisableInput();

        private void Update()
        {
            if (Gamepad.current == null) return;
            
            var stick = Gamepad.current.leftStick.ReadValue();
            
            if (stick.sqrMagnitude > 0.2f)
            {
                UseGamepadScheme();
            }
        }
        #endregion

        #region Private Methods

        private void UseGamepadScheme()
        {
            if (_currentScheme == GamepadScheme)
                return;

            _currentScheme = GamepadScheme;
            _controls.asset.bindingMask = InputBinding.MaskByGroup(GamepadScheme);
        }

        private void UseKeyboardMouseScheme()
        {
            if (_currentScheme == MouseAndKeyboardScheme)
                return;

            _currentScheme = MouseAndKeyboardScheme;
            _controls.asset.bindingMask = InputBinding.MaskByGroup(MouseAndKeyboardScheme);
        }
        
        private void OnAimPerformed(InputAction.CallbackContext ctx)
        {
            var input = ctx.ReadValue<Vector2>();

            if (_currentScheme == GamepadScheme)
                OnGamepadAimChanged?.Invoke(input);
            else
                OnMousePositionChanged?.Invoke(input);
        }

        
        private void OnAimCanceled(InputAction.CallbackContext ctx)
        {
            OnGamepadAimStopped?.Invoke();
        }

        
        private void BindInputActions(PlayerControls controls)
        {
            controls.Player.Move.performed += OnMoveOnPerformed;
            controls.Player.Move.canceled += OnMoveOnCanceled;

            controls.Player.Aim.performed += OnAimPerformed;
            controls.Player.Aim.canceled += OnAimCanceled;
            
            controls.Player.MouseActivity.performed += _ => UseKeyboardMouseScheme();

            controls.Player.Dash.performed += ctx => OnDashPerformed?.Invoke();
            controls.Player.SimpleAttack.performed += ctx => OnSimpleAbilityPerformed?.Invoke();
            controls.Player.SpecialAttack.performed += ctx => OnSpecialAbilityPerformed?.Invoke();
            
            controls.Player.Eat.performed += ctx => OnEatPerformed?.Invoke();
            
            controls.Player.ZoomOut.performed += ctx => OnZoomStarted?.Invoke();
            controls.Player.ZoomOut.canceled += ctx => OnZoomStopped?.Invoke();
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
            OnInputDisabled?.Invoke();
        }

        public void EnableInput()
        {
            _controls.Enable();
            OnInputEnabled?.Invoke();
        }
        
        #endregion
    }
}