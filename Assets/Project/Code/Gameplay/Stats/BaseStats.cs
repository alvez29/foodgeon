using Project.Code.Core;
using Project.Code.Core.Interfaces;
using UnityEngine;

namespace Project.Code.Gameplay.Stats
{
    public abstract class BaseStats : MonoBehaviour, IDamageable
    {
        [Header("Base Stats")]
        [SerializeField] private float maxHealth = 100f;
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
        public event System.Action OnDeath;

        protected virtual void Awake()
        {
            CurrentHealth = MaxHealth;
        }

        public virtual void TakeDamage(float amount, GameObject source)
        {
            if (CurrentHealth <= 0) return;

            var damageTaken = Mathf.Max(amount - Defense, 1f);
            
            CurrentHealth = Mathf.Clamp(CurrentHealth - damageTaken, 0, MaxHealth);
            OnHealthChanged?.Invoke(CurrentHealth, MaxHealth);

            if (CurrentHealth <= 0)
            {
                Die();
            }
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
            TakeDamage(10f, gameObject);
        }
    }
}