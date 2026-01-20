using System;
using Project.Code.Gameplay.Player;
using Project.Code.Gameplay.Player.Eating;
using Project.Code.Gameplay.Player.Stats;
using Project.Code.Gameplay.States.StatesLibrary.Player;
using Project.Code.Gameplay.States.StatesLibrary.Player.Grounded;
using UnityEngine;

namespace Project.Code.Gameplay.States
{
    [RequireComponent(typeof(PlayerDashAbility))]
    [RequireComponent(typeof(PlayerMovementComponent))]
    [RequireComponent(typeof(PlayerEatingComponent))]
    [RequireComponent(typeof(PlayerStats))]
    public class PlayerStateManager : StateManager
    {
        #region Fields
        
        // Components
        [NonSerialized] public PlayerDashAbility DashAbility;
        [NonSerialized] public PlayerMovementComponent MovementComponent;
        [NonSerialized] public PlayerEatingComponent PlayerEatingComponent;
        [NonSerialized] public PlayerStats PlayerStats;
        [NonSerialized] public PlayerInputHandler InputHandler;

        // States
        public readonly PlayerIdleState PlayerIdleState = new();
        public readonly PlayerRunState PlayerRunState = new();
        public readonly PlayerDashState PlayerDashState = new();
        public readonly PlayerEatingState PlayerEatingState = new();
        public readonly PlayerHitState PlayerHitState = new();
        
        #endregion

        #region Override Methods

        protected override void BindComponents()
        {
            base.BindComponents();
            
            DashAbility = GetComponent<PlayerDashAbility>();
            MovementComponent = GetComponent<PlayerMovementComponent>();
            PlayerEatingComponent = GetComponent<PlayerEatingComponent>();
            PlayerStats = GetComponent<PlayerStats>();
            InputHandler = GetComponent<PlayerInputHandler>();
        }

        protected override void SetDefaultState()
        {
            CurrentState = PlayerIdleState;
        }
        
        #endregion

    }
}
