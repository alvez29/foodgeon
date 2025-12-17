using Project.Code.Core;
using Project.Code.Core.Interfaces;
using Project.Code.Gameplay.Managers;
using Unity.Mathematics.Geometry;
using UnityEngine;

namespace Project.Code.Gameplay.Stats
{
    public abstract class BaseStats : MonoBehaviour, IDamageable
    {
        [Header("Base Stats")]
        [SerializeField] private float maxHealth = 10f;
        [SerializeField] private float strength = 10f;
        [SerializeField] private float defense = 5f;
        [SerializeField] private float speed = 5f;

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

        public event System.Action<float, float> OnHealthChanged;
        public event System.Action<float, float, float, GameObject> OnDamageTaken;
        public event System.Action OnDeath;

        protected virtual void Awake()
        {
            CurrentHealth = MaxHealth;
        }
        
        public virtual float TakeDamage(float amount, float abilityPower, GameObject source)
        {
            if (CurrentHealth <= 0) return 0f;

            // TODO: Refactor this formula 
            var damageTaken =
                Mathf.Clamp(Mathf.Floor((float)(((amount - (Defense / 1.2)) / 2.5) + ((abilityPower + 3.0) / 3.0))), 1,
                    10);

            CurrentHealth = Mathf.Clamp(CurrentHealth - damageTaken, 0, MaxHealth);
            OnHealthChanged?.Invoke(CurrentHealth, MaxHealth);
            HitStopManager.Instance.HitStop(0.1f);

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
        
        public virtual void Heal(float amount)
        {
            if (CurrentHealth <= 0) return;

            CurrentHealth = Mathf.Clamp(CurrentHealth + amount, 0, MaxHealth);
            OnHealthChanged?.Invoke(CurrentHealth, MaxHealth);
        }

        protected virtual void Die()
        {
            OnDeath?.Invoke();
        }

        protected void SetMaxHealth(float value) => maxHealth = value;

        [ContextMenu("Test Damage (10)")]
        private void TestDamage()
        {
            TakeDamage(1f, 2f, gameObject);
        }
    }
}