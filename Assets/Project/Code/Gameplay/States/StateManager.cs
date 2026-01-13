using System;
using Project.Code.Gameplay.Stats;
using UnityEngine;

namespace Project.Code.Gameplay.States
{
    // This manager should work as a router
    [RequireComponent(typeof(BaseStats))]
    public abstract class StateManager : MonoBehaviour
    {
        #region Fields
        
        public BaseState CurrentState;
        public BaseStats ownerStats;
        
        #endregion

        #region Abstract and Virtual Methods

        protected abstract void SetDefaultState();

        protected virtual void BindEvents()
        {
            if (ownerStats != null) ownerStats.OnDamageTaken += OnOwnerStatsOnOnDamageTaken;
        }

        private void OnOwnerStatsOnOnDamageTaken(float currentHealth, float maxHealth, float amount, GameObject source)
        {
            CurrentState?.OnDamageTaken(this, currentHealth, maxHealth, amount, source);
        }

        protected virtual void BindComponents()
        {
            ownerStats =  GetComponent<BaseStats>();
        }

        #endregion

        #region Unity Functions

        private void Awake()
        {
            BindComponents();
            BindEvents();
        }

        private void Start()
        {
            SetDefaultState();
            CurrentState?.OnStateEntered(this);
        }

        private void OnDisable()
        {
            if (ownerStats != null) ownerStats.OnDamageTaken -= OnOwnerStatsOnOnDamageTaken;
        }

        private void Update()
        {
            // ReSharper disable once Unity.PerformanceCriticalCodeInvocation
            CurrentState?.UpdateState(this);
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
