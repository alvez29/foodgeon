using Project.Code.Gameplay.Player;
using Project.Code.Gameplay.States.StatesLibrary.Player;
using Project.Code.Gameplay.States.StatesLibrary.Player.Grounded;
using UnityEngine;

namespace Project.Code.Gameplay.States
{
    [RequireComponent(typeof(PlayerDashAbility))]
    [RequireComponent(typeof(PlayerMovementComponent))]
    public class PlayerStateManager : StateManager
    {
        #region Fields
        
        // Components
        public PlayerDashAbility dashAbility;
        public PlayerMovementComponent movementComponent;
        public PlayerInputHandler inputHandler;
        
        // States
        public readonly PlayerIdleState PlayerIdleState = new PlayerIdleState();
        public readonly PlayerRunState PlayerRunState = new PlayerRunState();
        public readonly PlayerDashState PlayerDashState = new PlayerDashState();
        
        #endregion

        #region Override Methods

        protected override void BindComponents()
        {
            base.BindComponents();
            
            dashAbility = GetComponent<PlayerDashAbility>();
            movementComponent = GetComponent<PlayerMovementComponent>();
            inputHandler = GetComponent<PlayerInputHandler>();
        }

        protected override void SetDefaultState()
        {
            CurrentState = PlayerIdleState;
        }
        
        #endregion
    }
}
