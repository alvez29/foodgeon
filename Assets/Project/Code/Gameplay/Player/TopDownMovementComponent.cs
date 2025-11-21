using System;
using UnityEngine;

namespace Project.Code.Gameplay.Player
{
    [RequireComponent(typeof(CharacterController))]
    [RequireComponent(typeof(PlayerInputHandler))]
    public class TopDownMovementComponent : MonoBehaviour
    {
        // TODO: This should be in StatsHandler

        [Header("Movement Settings")]
        public float moveSpeed = 6f;

        public float acceleration = 3f;
        public float deceleration = 12f;
        public float rotationSpeed = 10f;

        [HideInInspector] public float currentSpeed = 0f;

        private const float MovementInputThreshold = 0.001f;
        
        private Vector3 _moveDirection = Vector3.zero;
        
        private CharacterController _controller;
        private PlayerInputHandler _inputHandler;

        private void Awake()
        {
            _controller = GetComponent<CharacterController>();
            _inputHandler = GetComponent<PlayerInputHandler>();
        }

        private void Update()
        {
            PerformMovement();
        }

        private void PerformMovement()
        {
            var inputVector = _inputHandler.MoveInput;
            var inputMagnitude = Mathf.Clamp01(inputVector.magnitude);

            if (inputMagnitude > MovementInputThreshold)
            {
                var targetDuration = new Vector3(inputVector.x, 0f, inputVector.y).normalized;
                _moveDirection = targetDuration;
                
                currentSpeed = Mathf.Lerp(currentSpeed, moveSpeed * inputMagnitude, acceleration * Time.deltaTime);
                
                transform.rotation = Quaternion.Slerp(
                    transform.rotation,
                    Quaternion.LookRotation(_moveDirection),
                    rotationSpeed * Time.deltaTime
                );
            }
            else
            {
                currentSpeed = Mathf.Lerp(currentSpeed, 0f, deceleration * Time.deltaTime);
            }
            
            _controller.Move(_moveDirection * (currentSpeed * Time.deltaTime));
        }
    }
}