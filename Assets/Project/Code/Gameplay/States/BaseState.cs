using UnityEngine;

namespace Project.Code.Gameplay.States
{
    public abstract class BaseState
    {
        #region Abstract Methods
        
        public abstract void OnStateEntered(StateManager contextManager);
        public abstract void UpdateState(StateManager contextManager);
        public abstract void OnStateExited(StateManager contextManager);
        public abstract void OnDamageTaken(StateManager contextManager, float currentHealth, float maxHealth, float amount, GameObject source);
        public abstract void OnCollisionEnter(StateManager contextManager);

        #endregion
    }
}
