using UnityEngine;

namespace Project.Code.Gameplay.Combat.Abilities.Base
{
    public abstract class Ability : ScriptableObject
    {
        [Header("Ability Settings")]
        [SerializeField] private string abilityName;
        [SerializeField] private float cooldown = 1f;

        public string AbilityName => abilityName;
        public float Cooldown => cooldown;
        
        public abstract void Activate(GameObject subject);
    }
}
