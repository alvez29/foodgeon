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
        public PlayerDashAbility dashAbility;
        public PlayerMovementComponent movementComponent;
        public PlayerEatingComponent playerEatingComponent;
        public PlayerStats playerStats;
        public PlayerInputHandler inputHandler;

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
            
            dashAbility = GetComponent<PlayerDashAbility>();
            movementComponent = GetComponent<PlayerMovementComponent>();
            playerEatingComponent = GetComponent<PlayerEatingComponent>();
            playerStats = GetComponent<PlayerStats>();
            inputHandler = GetComponent<PlayerInputHandler>();
        }

        protected override void SetDefaultState()
        {
            CurrentState = PlayerIdleState;
        }
        
        #endregion

    }
}
