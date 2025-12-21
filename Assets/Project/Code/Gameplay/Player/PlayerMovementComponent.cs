using System;
using Project.Code.Core;
using UnityEngine;
using Project.Code.Gameplay.Stats;

namespace Project.Code.Gameplay.Player
{
    [RequireComponent(typeof(CharacterController))]
    [RequireComponent(typeof(PlayerInputHandler))]
    [RequireComponent(typeof(BaseStats))]
    public class PlayerMovementComponent : MonoBehaviour
    {
        #region Serialized Fields
        
        [Header("Movement Setting")] 
        [SerializeField] private float acceleration = 3f;
        [SerializeField] private float deceleration = 12f;
        [SerializeField] private float rotationSpeed = 10f;
        
        #endregion

        #region Properties
        
        private float CurrentSpeed { get; set; } = 0f;
        public Vector3 MoveDirection { get; private set; } = Vector3.zero;
        private Vector3 TargetDirection { get; set; } = Vector3.zero;
        public bool IsMoving => CurrentSpeed > Constants.Movement.MovementInputThreshold;
        
        #endregion

        #region Fields

        private float _targetSpeed = 0f;

        private CharacterController _controller;
        private PlayerInputHandler _inputHandler;
        private BaseStats _stats;
        
        #endregion

        #region Unity Functions

        private void Awake()
        {
            _controller = GetComponent<CharacterController>();
            _inputHandler = GetComponent<PlayerInputHandler>();
            _stats = GetComponent<BaseStats>();
        }

        private void OnEnable()
        {
            _inputHandler.OnMoveInputChanged += HandleMoveInputChanged;
            _inputHandler.OnAimInputChanged += HandleAimInputChanged;
        }

        private void OnDisable()
        {
            _inputHandler.OnMoveInputChanged -= HandleMoveInputChanged;
        }
        
        private void Update()
        {
            UpdateMovement();
        }
        
        #endregion

        #region Private Methods

        private void HandleAimInputChanged(Vector2 aimVector)
        {
            TargetDirection = new Vector3(aimVector.x, 0f, aimVector.y).normalized;
        }

        private void HandleMoveInputChanged(Vector2 input)
        {
            var inputMagnitude = Mathf.Clamp01(input.magnitude);

            if (inputMagnitude > Constants.Movement.MovementInputThreshold)
            {
                var finalSpeed = _stats ? _stats.Speed : 0f;
                _targetSpeed = finalSpeed * inputMagnitude;
                MoveDirection = new Vector3(input.x, 0f, input.y).normalized;
            }
            else
            {
                _targetSpeed = 0f;
            }
        }

        private void UpdateMovement()
        {
            var speedChange = _targetSpeed > CurrentSpeed ? acceleration : deceleration;
            CurrentSpeed = Mathf.Lerp(CurrentSpeed, _targetSpeed, speedChange * Time.deltaTime);
            
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                Quaternion.LookRotation(TargetDirection),
                rotationSpeed * Time.deltaTime
            );
            
            _controller.Move(MoveDirection * (CurrentSpeed * Time.deltaTime));
        }
        
        #endregion
    }
}