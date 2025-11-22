using System.Collections;
using UnityEngine;

namespace Project.Code.Gameplay.Player
{
    [RequireComponent(typeof(CharacterController))]
    [RequireComponent(typeof(PlayerInputHandler))]
    public class PlayerDashAbility : MonoBehaviour
    {
        [Header("Dash Settings")]
        [SerializeField] private float dashDistance = 5f;
        [SerializeField] private float dashDuration = 0.2f;
        [SerializeField] private float dashCooldown = 1f;
        [SerializeField] private AnimationCurve dashCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

        private bool IsDashing { get; set; }
        private bool CanDash => !IsDashing && Time.time >= _lastDashTime + dashCooldown;

        private float _lastDashTime = -999f;
        
        private CharacterController _controller;
        private PlayerInputHandler _inputHandler;
        private MovementComponent _movementComponent;

        private void Awake()
        {
            _controller = GetComponent<CharacterController>();
            _inputHandler = GetComponent<PlayerInputHandler>();
            _movementComponent = GetComponent<MovementComponent>();
        }

        private void OnEnable()
        {
            _inputHandler.OnDashPerformed += TryPerformDash;
        }

        private void OnDisable()
        {
            _inputHandler.OnDashPerformed -= TryPerformDash;
        }

        private void TryPerformDash()
        {
            if (CanDash)
            {
                StartCoroutine(PerformDash());
            }
        }

        private IEnumerator PerformDash()
        {
            IsDashing = true;
            _lastDashTime = Time.time;
            
            var dashDirection = _movementComponent.IsMoving 
                ? _movementComponent.MoveDirection 
                : transform.forward;

            var elapsedTime = 0f;

            while (elapsedTime < dashDuration)
            {
                var t = elapsedTime / dashDuration;
                var curveValue = dashCurve.Evaluate(t);
                var dashSpeed = (dashDistance / dashDuration) * curveValue;

                _controller.Move(dashDirection * (dashSpeed * Time.deltaTime));

                elapsedTime += Time.deltaTime;
                yield return null;
            }

            IsDashing = false;
        }
    }
}