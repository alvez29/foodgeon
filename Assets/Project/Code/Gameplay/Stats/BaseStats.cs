using Project.Code.Core.Interfaces;
using UnityEngine;

namespace Project.Code.Gameplay.Stats
{
    public abstract class BaseStats : MonoBehaviour, IDamageable
    {
        [Header("Base Stats")]
        [SerializeField] private float _maxHealth = 100f;
        [SerializeField] private float _strength = 10f;
        [SerializeField] private float _defense = 5f;
        [SerializeField] private float _speed = 5f;

        public float CurrentHealth { get; private set; }
        public float MaxHealth => _maxHealth;
        public float Strength => _strength;
        public float Defense => _defense;
        public float Speed => _speed;

        public event System.Action<float, float> OnHealthChanged; // Current, Max
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

        // Allow child classes to modify stats
        protected void SetSpeed(float value) => _speed = value;
        protected void SetMaxHealth(float value) => _maxHealth = value;
    }
}