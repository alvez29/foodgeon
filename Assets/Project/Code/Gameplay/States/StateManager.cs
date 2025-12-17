using Project.Code.Gameplay.Stats;
using UnityEngine;

namespace Project.Code.Gameplay.States
{
    [RequireComponent(typeof(BaseStats))]
    public abstract class StateManager : MonoBehaviour
    {
        public BaseState CurrentState;
        
        public BaseStats ownerStats;

        protected virtual void BindComponents()
        {
            ownerStats = GetComponent<BaseStats>();
            // Derived classes will bind their own states/components
        }

        protected abstract void SetDefaultState();
        
        private void Awake()
        {
            BindComponents();
        }

        private void Start()
        {
            SetDefaultState();
            CurrentState?.OnStateEntered(this);
        }

        private void Update()
        {
            CurrentState?.UpdateState(this);
        }

        public void SwitchState(BaseState newState)
        {
            CurrentState?.OnStateExited(this);
            CurrentState = newState;
            CurrentState.OnStateEntered(this);
        }
    }
}
