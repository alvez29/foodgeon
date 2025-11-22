using System;
using InputActions;
using UnityEngine;

namespace Project.Code.Gameplay.Player
{
    public class PlayerInputHandler : MonoBehaviour
    {
        public event Action OnDashPerformed;
        public event Action OnSimpleAttackPerformed;
        public event Action OnSpecialAttackPerformed;
        public event Action<Vector2> OnMoveInputChanged;
        
        public Vector2 MoveInput { get; private set; }
        
        private PlayerControls _controls;
        
        private void Awake()
        {
            _controls = new PlayerControls();
            BindInputActions();
        }

        private void BindInputActions()
        {
            _controls.Player.Move.performed += ctx => 
            {
                MoveInput = ctx.ReadValue<Vector2>();
                OnMoveInputChanged?.Invoke(MoveInput);
            };
            
            _controls.Player.Move.canceled += ctx => 
            {
                MoveInput = Vector2.zero;
                OnMoveInputChanged?.Invoke(MoveInput);
            };

            _controls.Player.Dash.performed += ctx => OnDashPerformed?.Invoke();
            _controls.Player.SimpleAttack.performed += ctx => OnSimpleAttackPerformed?.Invoke();
            _controls.Player.SpecialAttack.performed += ctx => OnSpecialAttackPerformed?.Invoke();
        }

        private void OnEnable() => _controls?.Enable();
        private void OnDisable() => _controls?.Disable();
    }
}