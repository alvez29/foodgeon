using UnityEngine;

namespace Project.Code.Gameplay.States
{
    public abstract class PlayerBaseState : BaseState
    {
        #region Fields

        private PlayerStateManager _playerStateManager;
        
        #endregion

        #region Override Methods

        public override void OnStateEntered(StateManager contextManager)
        {
            _playerStateManager = contextManager as PlayerStateManager;
            if (!_playerStateManager)
            {
                Debug.LogError($"State {GetType().Name} expected PlayerStateManager!");
                return;
            }
            OnPlayerStateEntered(_playerStateManager);
        }

        public override void UpdateState(StateManager contextManager)
        {
            if (_playerStateManager) OnPlayerStateUpdate(_playerStateManager);
        }

        public override void OnStateExited(StateManager contextManager)
        {
            if (_playerStateManager) OnPlayerStateExited(_playerStateManager);
            
            _playerStateManager = null;
        }

        public override void OnCollisionEnter(StateManager contextManager)
        {
            if (_playerStateManager != null) OnPlayerStateCollision(_playerStateManager);
        }

        public override void OnDamageTaken(StateManager contextManager, float currentHealth, float maxHealth, float amount, GameObject source)
        {
            if (_playerStateManager != null) OnPlayerDamageTaken(_playerStateManager, currentHealth, maxHealth, amount, source);
        }

        #endregion

        #region Virtual Methods

        // Virtual methods for subclasses
        protected virtual void OnPlayerStateEntered(PlayerStateManager manager) { }
        protected virtual void OnPlayerStateUpdate(PlayerStateManager manager) { }
        protected virtual void OnPlayerStateExited(PlayerStateManager manager) { }
        protected virtual void OnPlayerStateCollision(PlayerStateManager manager) { }
        protected virtual void OnPlayerDamageTaken(PlayerStateManager manager , float currentHealth, float maxHealth, float amount, GameObject source)
        {
            manager.SwitchState(manager.PlayerHitState);
        }
        
        #endregion
    }
}
