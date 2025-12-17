namespace Project.Code.Gameplay.States
{
    public abstract class BaseState
    {
        public abstract void OnStateEntered(StateManager contextManager);
        public abstract void UpdateState(StateManager contextManager);
        public abstract void OnStateExited(StateManager contextManager);
        public abstract void OnCollisionEnter(StateManager contextManager);
    }
}
