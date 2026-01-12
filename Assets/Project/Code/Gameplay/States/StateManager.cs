using Project.Code.Gameplay.Stats;
using UnityEngine;

namespace Project.Code.Gameplay.States
{
    [RequireComponent(typeof(BaseStats))]
    public abstract class StateManager : MonoBehaviour
    {
        #region Fields
        
        public BaseState CurrentState;
        public BaseStats ownerStats;
        
        #endregion

        #region Abstract Methods

        protected abstract void SetDefaultState();
        
        #endregion

        #region Unity Functions

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
            // ReSharper disable once Unity.PerformanceCriticalCodeInvocation
            CurrentState?.UpdateState(this);
        }
        
        #endregion

        #region Protected Methods

        protected virtual void BindComponents()
        {
            ownerStats = GetComponent<BaseStats>();
            // Derived classes should bind their own states/components
        }
        
        #endregion

        #region Public Methods

        // ReSharper disable Unity.PerformanceAnalysis
        public void SwitchState(BaseState newState)
        {
            CurrentState?.OnStateExited(this);
            CurrentState = newState;
            CurrentState.OnStateEntered(this);
        }
        
        #endregion
    }
}
