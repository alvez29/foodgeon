using System;
using InputActions;
using UnityEngine;

namespace Project.Code.Gameplay.Player
{
    public class PlayerInputHandler : MonoBehaviour
    {
        public Vector2 MoveInput { get; private set; }
        public bool DashTriggered { get; private set; }
        public bool SimpleAttackTriggered { get; private set; }
        public bool SpecialAttackTriggered { get; private set;}
        
        private PlayerControls _controls;
        
        private void Awake()
        {
            _controls = new PlayerControls();
            
            // Move
            _controls.Player.Move.performed += ctx => MoveInput = ctx.ReadValue<Vector2>();
            _controls.Player.Move.canceled += ctx => MoveInput = Vector2.zero;

            // Dash
            _controls.Player.Dash.performed += ctx => DashTriggered = true;

            // Attacks
            _controls.Player.SimpleAttack.performed += ctx => SimpleAttackTriggered = true;
            _controls.Player.SpecialAttack.performed += ctx => SpecialAttackTriggered = true;
        }

        private void OnEnable() => _controls.Enable();
        private void OnDisable() => _controls.Disable();

        private void LateUpdate()
        {
            DashTriggered = false;
            SimpleAttackTriggered = false;
            SpecialAttackTriggered = false;
        }
    }
}
