using Project.Code.Core;
using UnityEngine;
using Project.Code.Gameplay.Stats;

namespace Project.Code.Gameplay.Player
{
    [RequireComponent(typeof(CharacterController))]
    [RequireComponent(typeof(PlayerInputHandler))]
    [RequireComponent(typeof(BaseStats))]
    public class MovementComponent : MonoBehaviour
    {
        [Header("Movement Setting")] 
        [SerializeField] private float moveSpeed = 0f;
        [SerializeField] private float acceleration = 3f;
        [SerializeField] private float deceleration = 12f;
        [SerializeField] private float rotationSpeed = 10f;
        
        private float CurrentSpeed { get; set; } = 0f;
        public Vector3 MoveDirection { get; private set; } = Vector3.zero;
        public bool IsMoving => CurrentSpeed > Constants.Movement.MovementInputThreshold;

        private Vector3 _targetDirection = Vector3.zero;
        private float _targetSpeed = 0f;

        private CharacterController _controller;
        private PlayerInputHandler _inputHandler;
        private BaseStats _stats;

        private void Awake()
        {
            _controller = GetComponent<CharacterController>();
            _inputHandler = GetComponent<PlayerInputHandler>();
            _stats = GetComponent<BaseStats>();
        }

        private void OnEnable()
        {
            _inputHandler.OnMoveInputChanged += HandleMoveInputChanged;
        }

        private void OnDisable()
        {
            _inputHandler.OnMoveInputChanged -= HandleMoveInputChanged;
        }

        private void HandleMoveInputChanged(Vector2 input)
        {
            var inputMagnitude = Mathf.Clamp01(input.magnitude);

            if (inputMagnitude > Constants.Movement.MovementInputThreshold)
            {
                _targetDirection = new Vector3(input.x, 0f, input.y).normalized;

                var finalSpeed = moveSpeed;
                
                if (_stats != null)
                {
                    finalSpeed = _stats.Speed;
                }

                _targetSpeed = finalSpeed * inputMagnitude;
            }
            else
            {
                _targetSpeed = 0f;
            }
        }

        private void Update()
        {
            UpdateMovement();
        }

        private void UpdateMovement()
        {
            var speedChange = _targetSpeed > CurrentSpeed ? acceleration : deceleration;
            CurrentSpeed = Mathf.Lerp(CurrentSpeed, _targetSpeed, speedChange * Time.deltaTime);

            if (CurrentSpeed > Constants.Movement.MovementInputThreshold)
            {
                MoveDirection = _targetDirection;

                transform.rotation = Quaternion.Slerp(
                    transform.rotation,
                    Quaternion.LookRotation(MoveDirection),
                    rotationSpeed * Time.deltaTime
                );
            }

            _controller.Move(MoveDirection * (CurrentSpeed * Time.deltaTime));
        }

        public void SetSpeedMultiplier(float multiplier)
        {
            _targetSpeed *= multiplier;
        }
    }
}