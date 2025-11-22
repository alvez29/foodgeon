using UnityEngine;

namespace Project.Code.Gameplay.Player
{
    [RequireComponent(typeof(CharacterController))]
    [RequireComponent(typeof(PlayerInputHandler))]
    public class MovementComponent : MonoBehaviour
    {
        [Header("Movement Setting")]
        [SerializeField] private float moveSpeed = 6f;
        [SerializeField] private float acceleration = 3f;
        [SerializeField] private float deceleration = 12f;
        [SerializeField] private float rotationSpeed = 10f;

        public float CurrentSpeed => _currentSpeed;
        public Vector3 MoveDirection => _moveDirection;
        public bool IsMoving => _currentSpeed > MovementInputThreshold;

        private const float MovementInputThreshold = 0.001f;
        
        private Vector3 _moveDirection = Vector3.zero;
        private Vector3 _targetDirection = Vector3.zero;
        private float _currentSpeed = 0f;
        private float _targetSpeed = 0f;
        
        private CharacterController _controller;
        private PlayerInputHandler _inputHandler;

        private void Awake()
        {
            _controller = GetComponent<CharacterController>();
            _inputHandler = GetComponent<PlayerInputHandler>();
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
            
            if (inputMagnitude > MovementInputThreshold)
            {
                _targetDirection = new Vector3(input.x, 0f, input.y).normalized;
                _targetSpeed = moveSpeed * inputMagnitude;
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
            var speedChange = _targetSpeed > _currentSpeed ? acceleration : deceleration;
            _currentSpeed = Mathf.Lerp(_currentSpeed, _targetSpeed, speedChange * Time.deltaTime);
            
            if (_currentSpeed > MovementInputThreshold)
            {
                _moveDirection = _targetDirection;
                
                transform.rotation = Quaternion.Slerp(
                    transform.rotation,
                    Quaternion.LookRotation(_moveDirection),
                    rotationSpeed * Time.deltaTime
                );
            }
            
            _controller.Move(_moveDirection * (_currentSpeed * Time.deltaTime));
        }

        public void SetSpeedMultiplier(float multiplier)
        {
            _targetSpeed = moveSpeed * multiplier;
        }

        public void StopMovement()
        {
            _currentSpeed = 0f;
            _targetSpeed = 0f;
        }
    }
}