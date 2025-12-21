using Project.Code.Gameplay.Player;
using UnityEngine;

namespace Project.Code.Gameplay.States
{
    public abstract class PlayerBaseState : BaseState
    {
        #region Fields
        
        protected PlayerStateManager PlayerStateManager;
        
        #endregion

        #region Override Methods

        public override void OnStateEntered(StateManager contextManager)
        {
            PlayerStateManager = contextManager as PlayerStateManager;
            if (!PlayerStateManager)
            {
                Debug.LogError($"State {GetType().Name} expected PlayerStateManager!");
                return;
            }
            OnPlayerStateEntered(PlayerStateManager);
        }

        public override void UpdateState(StateManager contextManager)
        {
            if (PlayerStateManager) OnPlayerStateUpdate(PlayerStateManager);
        }

        public override void OnStateExited(StateManager contextManager)
        {
            if (PlayerStateManager) OnPlayerStateExited(PlayerStateManager);
            
            PlayerStateManager = null;
        }

        public override void OnCollisionEnter(StateManager contextManager)
        {
            if (PlayerStateManager != null) OnPlayerStateCollision(PlayerStateManager);
        }
        
        #endregion

        #region Virtual Methods

        // Virtual methods for subclasses
        protected virtual void OnPlayerStateEntered(PlayerStateManager manager) { }
        protected virtual void OnPlayerStateUpdate(PlayerStateManager manager) { }
        protected virtual void OnPlayerStateExited(PlayerStateManager manager) { }
        protected virtual void OnPlayerStateCollision(PlayerStateManager manager) { }
        
        #endregion
    }
}
