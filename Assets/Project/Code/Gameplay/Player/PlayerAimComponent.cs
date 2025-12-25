using System;
using System.Collections;
using Project.Code.Core;
using UnityEngine;

namespace Project.Code.Gameplay.Player
{
    [RequireComponent(typeof(PlayerInputHandler))]
    public class PlayerAimComponent : MonoBehaviour
    {
        #region Constants
        
        private const float AimIdleTimeout = 2.0f;
        
        #endregion

        #region Events
        
        public event Action<Vector2> OnAimDirectionChanged;
        
        #endregion

        #region Properties

        public bool IsUsingMouse { get; private set; }
        public Vector2 AimDirection { get; private set; }
        public Vector3 AimDirection3D { get; private set; }
        public Vector3 MouseWorldPosition { get; private set; }
        
        #endregion

        #region Fields

        private Vector2 _moveInput;
        private bool _isAimingWithGamepad;
        private Coroutine _aimTimeoutCoroutine;
        
        private PlayerInputHandler _inputHandler;
        private UnityEngine.Camera _mainCamera;
        
        #endregion
        
        #region Unity Functions

        private void Awake()
        {
            _inputHandler = GetComponent<PlayerInputHandler>();
            _mainCamera = UnityEngine.Camera.main;
        }

        private void OnEnable()
        {
            _inputHandler.OnMousePositionChanged += HandleMousePosition;
            _inputHandler.OnGamepadAimChanged += HandleGamepadAim;
            _inputHandler.OnGamepadAimStopped += HandleGamepadAimStopped;
            _inputHandler.OnMoveInputChanged += HandleMoveInput;
        }

        private void OnDisable()
        {
            _inputHandler.OnMousePositionChanged -= HandleMousePosition;
            _inputHandler.OnGamepadAimChanged -= HandleGamepadAim;
            _inputHandler.OnGamepadAimStopped -= HandleGamepadAimStopped;
            _inputHandler.OnMoveInputChanged -= HandleMoveInput;
        }
        
        #endregion

        #region Private Methods

        private void HandleMousePosition(Vector2 screenPosition)
        {
            IsUsingMouse = true;
            _isAimingWithGamepad = false;
            
            if (_aimTimeoutCoroutine != null)
            {
                StopCoroutine(_aimTimeoutCoroutine);
                _aimTimeoutCoroutine = null;
            }

            if (!_mainCamera) return;

            var ray = _mainCamera.ScreenPointToRay(screenPosition);
            var groundPlane = new Plane(Vector3.up, transform.position);

            if (!groundPlane.Raycast(ray, out var enter)) return;
            
            var hitPoint = ray.GetPoint(enter);
            MouseWorldPosition = hitPoint;
            
            var direction3D = hitPoint - transform.position;
            var direction2D = new Vector2(direction3D.x, direction3D.z).normalized;
            
            UpdateAimDirection(direction2D, direction3D.normalized);
        }

        private void HandleGamepadAim(Vector2 input)
        {
            IsUsingMouse = false;
            
            if (_aimTimeoutCoroutine != null)
            {
                StopCoroutine(_aimTimeoutCoroutine);
                _aimTimeoutCoroutine = null;
            }
            
            var shouldApplyAiming = input.sqrMagnitude > Constants.Movement.AimInputThreshold;
            _isAimingWithGamepad = shouldApplyAiming;

            if (shouldApplyAiming)
            {
                var direction2D = input.normalized;
                var direction3D = new Vector3(direction2D.x, 0f, direction2D.y).normalized;
                UpdateAimDirection(direction2D, direction3D);
            }
        }

        private void HandleGamepadAimStopped()
        {
            _isAimingWithGamepad = false;
            
            if (_aimTimeoutCoroutine != null)
            {
                StopCoroutine(_aimTimeoutCoroutine);
            }
            _aimTimeoutCoroutine = StartCoroutine(AimTimeoutRoutine());
        }

        private void HandleMoveInput(Vector2 input)
        {
            _moveInput = input;
            TryAimAtMovement();
        }

        private void TryAimAtMovement()
        {
            if (IsUsingMouse || _isAimingWithGamepad || _aimTimeoutCoroutine != null) return;
            
            if (_moveInput.sqrMagnitude > Constants.Movement.AimInputThreshold)
            {
                var direction2D = _moveInput.normalized;
                var direction3D = new Vector3(direction2D.x, 0f, direction2D.y).normalized;
                UpdateAimDirection(direction2D, direction3D);
            }
        }

        private IEnumerator AimTimeoutRoutine()
        {
            yield return new WaitForSeconds(AimIdleTimeout);
            _aimTimeoutCoroutine = null;
            TryAimAtMovement();
        }

        private void UpdateAimDirection(Vector2 direction2D, Vector3 direction3D)
        {
            AimDirection = direction2D;
            AimDirection3D = direction3D;
            OnAimDirectionChanged?.Invoke(direction2D);
        }
        
        #endregion
    }
}
