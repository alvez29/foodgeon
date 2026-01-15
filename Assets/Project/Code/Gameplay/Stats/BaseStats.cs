using Project.Code.Core;
using Project.Code.Core.Data;
using Project.Code.Core.Interfaces;
using Project.Code.Gameplay.Camera;
using Project.Code.Gameplay.Managers;
using Unity.Mathematics.Geometry;
using UnityEngine;

namespace Project.Code.Gameplay.Stats
{
    public abstract class BaseStats : MonoBehaviour, IDamageable
    {
        #region Events

        public event System.Action<float, float> OnHealthChanged;
        public event System.Action<float, float, float, GameObject> OnDamageTaken;
        public event System.Action OnDeath;
        
        #endregion
        
        #region Serialized Fields
        
        [Header("Base Stats")]
        [SerializeField] private float maxHealth = 10f;
        [SerializeField] private float strength = 10f;
        [SerializeField] private float defense = 5f;
        [SerializeField] private float speed = 5f;
        
        #endregion

        #region Properties

        public float CurrentHealth { get; private set; }
        public float MaxHealth => maxHealth;
        public float Strength
        {
            get => strength;
            private set => strength = value;
        }

        public float Defense
        {
            get => defense;
            private set => defense = value;
        }

        public float Speed
        {
            get => speed;
            private set => speed = value;
        }

        protected bool IsDead => CurrentHealth <= 0;
        private bool _isInvincible = false;
        
        #endregion

        #region Unity Functions

        protected virtual void Awake()
        {
            CurrentHealth = MaxHealth;
        }
        
        #endregion
        
        #region Public Methods
        
        public virtual float TakeDamage(float amount, float abilityPower, GameObject source)
        {
            if (_isInvincible || IsDead) return 0f;

            var damageTaken = Constants.Stats.GetDamageValue(amount, Defense, abilityPower);
            var hitStopDuration = Mathf.Clamp(damageTaken * 0.02f, 0.1f, 0.7f);
            var trauma = Mathf.Clamp(damageTaken * 0.03f, 0.5f, 0.8f);
            
            CurrentHealth = Mathf.Clamp(CurrentHealth - damageTaken, 0, MaxHealth);
            OnHealthChanged?.Invoke(CurrentHealth, MaxHealth);
            
            HitStopManager.Instance.HitStop(hitStopDuration);
            CameraShake.Instance.AddTrauma(trauma);

            if (damageTaken > 0) OnDamageTaken?.Invoke(CurrentHealth, MaxHealth, damageTaken, source);

            if (CurrentHealth <= 0)
            {
                Die();
            }

            return damageTaken;
        }

        public void AddDefense(float amount)
        {
            Defense += Mathf.Clamp(amount, 0, Constants.Stats.MaxDefense);
        }
        
        public void AddStrength(float amount)
        {
            Strength += Mathf.Clamp(amount, 0, Constants.Stats.MaxStrength);
        }
        
        public void AddSpeed(float amount)
        {
            Speed += Mathf.Clamp(amount, 0, Constants.Stats.MaxSpeed);
        }

        public void AddEnemyReward(EnemyReward enemyReward)
        {
            AddDefense(enemyReward.Defense);
            AddSpeed(enemyReward.Speed);
            AddStrength(enemyReward.Defense);
        }

        public void SetInvincibility(bool newInvincibility)
        {
            _isInvincible = newInvincibility;
        }
        
        public virtual void Heal(float amount)
        {
            if (CurrentHealth <= 0) return;

            CurrentHealth = Mathf.Clamp(CurrentHealth + amount, 0, MaxHealth);
            OnHealthChanged?.Invoke(CurrentHealth, MaxHealth);
        }
        
        #endregion

        #region Protected Methods

        protected virtual void Die()
        {
            OnDeath?.Invoke();
        }

        protected void SetMaxHealth(float value) => maxHealth = value;
        
        #endregion

        #region Private Methods

        [ContextMenu("Test Damage (10)")]
        private void TestDamage()
        {
            TakeDamage(1f, 2f, gameObject);
        }
        
        #endregion
    }
}