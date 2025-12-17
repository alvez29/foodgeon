using UnityEngine;

namespace Project.Code.Core.Data.ScriptableObjects
{
    /// <summary>
    /// Base class for all ability execution strategies.
    /// Defines how an ability is executed (melee, projectile, AOE, etc.)
    /// </summary>
    public abstract class AbilityExecutor : ScriptableObject
    {
        /// <summary>
        /// Execute the ability logic
        /// </summary>
        /// <param name="caster">The GameObject using the ability</param>
        /// <param name="data">The ability data containing parameters</param>
        public abstract void Execute(GameObject caster, AbilityData data);
        
        /// <summary>
        /// Called when the ability hits a target
        /// </summary>
        /// <param name="caster">The GameObject using the ability</param>
        /// <param name="target">The GameObject that was hit</param>
        /// <param name="data">The ability data containing parameters</param>
        public abstract void OnHit(GameObject caster, GameObject target, AbilityData data);
    }
}
