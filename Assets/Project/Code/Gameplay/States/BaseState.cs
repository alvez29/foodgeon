namespace Project.Code.Gameplay.States
{
    public abstract class BaseState
    {
        #region Abstract Methods
        
        public abstract void OnStateEntered(StateManager contextManager);
        public abstract void UpdateState(StateManager contextManager);
        public abstract void OnStateExited(StateManager contextManager);
        public abstract void OnCollisionEnter(StateManager contextManager);
        
        #endregion
    }
}
