using System;
using System.Collections;
using InputActions;
using Project.Code.Core;
using UnityEngine;
using UnityEngine.InputSystem;


namespace Project.Code.Gameplay.Player
{
    public class PlayerInputHandler : MonoBehaviour
    {
        #region Constants
        
        private const float AimIdleTimeout = 2.0f;
        
        #endregion

        #region Events
        
        public event Action OnDashPerformed;
        public event Action OnSimpleAbilityPerformed;
        public event Action OnSpecialAbilityPerformed;
        public event Action<Vector2> OnMoveInputChanged;
        public event Action<Vector2> OnAimInputChanged;
        
        #endregion

        #region Properties

        public bool IsUsingMouse { get; private set; }
        public Vector3 MouseRaycastPosition { get; private set; }
        public Vector2 AimInput { get; private set; }
        
        #endregion

        #region Fields

        private Vector2 MoveInput { get; set; }
        private Coroutine _aimTimeoutCoroutine;
        
        private PlayerControls _controls;
        private UnityEngine.Camera _mainCamera;
        
        #endregion
        
        #region Unity Functions

        private void Awake()
        {
            _controls = new PlayerControls();
            _mainCamera = UnityEngine.Camera.main;
            BindInputActions();
        }

        private void Start()
        {
            StartCoroutine(MousePollRoutine());
        }

        private void OnEnable() => _controls?.Enable();
        private void OnDisable() => _controls?.Disable();
        
        #endregion

        #region Private Methods

        private void OnGamepadAimPerformed(InputAction.CallbackContext ctx)
        {
            IsUsingMouse = false;
            if (_aimTimeoutCoroutine != null) StopCoroutine(_aimTimeoutCoroutine);
            
            var input = ctx.ReadValue<Vector2>();
            var shouldApplyAiming = input.sqrMagnitude > Constants.Movement.AimInputThreshold;

            if (shouldApplyAiming)
            {
                AimInput = input.normalized;
                OnAimInputChanged?.Invoke(AimInput);
            }
        }

        private void OnGamepadAimCanceled(InputAction.CallbackContext ctx)
        {
            if (!IsUsingMouse)
            {
                _aimTimeoutCoroutine = StartCoroutine(AimTimeoutRoutine());
            }
        }

        private IEnumerator AimTimeoutRoutine()
        {
            yield return new WaitForSeconds(AimIdleTimeout);

            if (!IsUsingMouse && MoveInput.sqrMagnitude > Constants.Movement.AimInputThreshold)
            {
                AimInput = MoveInput.normalized;
                OnAimInputChanged?.Invoke(AimInput);
            }    
        }

        private IEnumerator MousePollRoutine()
        {
            while (true)
            {
                if (Mouse.current != null && Mouse.current.delta.ReadValue().sqrMagnitude > 0.1f)
                {
                    IsUsingMouse = true;
                    if (_aimTimeoutCoroutine != null) StopCoroutine(_aimTimeoutCoroutine);
                }

                if (IsUsingMouse)
                {
                    HandleMouseAim();
                }

                yield return null;
            }
        }

        private void HandleMouseAim()
        {
            if (!_mainCamera) return;

            var ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
            var groundPlane = new Plane(Vector3.up, transform.position);

            if (!groundPlane.Raycast(ray, out var enter)) return;
            
            var hitPoint = ray.GetPoint(enter);
            MouseRaycastPosition = hitPoint;
            var direction3D = hitPoint - transform.position;
            var direction2D = new Vector2(direction3D.x, direction3D.z).normalized;
                
            AimInput = direction2D;
            OnAimInputChanged?.Invoke(AimInput);
        }

        private void BindInputActions()
        {
            _controls.Player.Move.performed += OnMoveOnPerformed;
            _controls.Player.Move.canceled += OnMoveOnCanceled;

            _controls.Player.Aim.performed += OnGamepadAimPerformed;
            _controls.Player.Aim.canceled += OnGamepadAimCanceled;

            _controls.Player.Dash.performed += ctx => OnDashPerformed?.Invoke();
            _controls.Player.SimpleAttack.performed += ctx => OnSimpleAbilityPerformed?.Invoke();
            _controls.Player.SpecialAttack.performed += ctx => OnSpecialAbilityPerformed?.Invoke();
        }

        private void OnMoveOnPerformed(InputAction.CallbackContext ctx)
        {
            MoveInput = ctx.ReadValue<Vector2>();
            OnMoveInputChanged?.Invoke(MoveInput);
        }

        private void OnMoveOnCanceled(InputAction.CallbackContext ctx)
        {
            MoveInput = Vector2.zero;
            OnMoveInputChanged?.Invoke(MoveInput);
        }
        
        #endregion
    }
}