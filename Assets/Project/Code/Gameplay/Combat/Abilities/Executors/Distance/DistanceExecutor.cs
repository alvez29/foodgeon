using Project.Code.Core;
using Project.Code.Core.Data.ScriptableObjects;
using Project.Code.Core.Interfaces;
using Project.Code.Gameplay.Stats;
using Project.Code.Utils;
using UnityEngine;

namespace Project.Code.Gameplay.Combat.Abilities.Executors.Distance
{
    /// <summary>
    /// Executor for melee cone-based attacks.
    /// Detects targets in a sphere around the caster and damage every enemy inside it.
    /// </summary>
    [CreateAssetMenu(fileName = "New Distance Executor", menuName = "Foodgeon/Executors/Distance Executor")]
    public class DistanceExecutor : AbilityExecutor
    {
        #region Fields
        
        [SerializeField] private float radius = 5f;
        
        private readonly Collider[] _hitResults = new Collider[20];
        
        #endregion

        #region Private Methods

        private float GetRangeValue(CharacterController controller)
        {
            return Constants.Stats.Radius(controller) + radius * Constants.Stats.RangeBaseUnit(controller);
        }
        
        #endregion
        
        #region Override Methods

        public override void Execute(GameObject caster, CharacterController controller, AbilityData data)
        {
            var origin = caster.transform.position;
            var abilityRange = GetRangeValue(controller);

            // Detect all colliders in range
            var hitCount = Physics.OverlapSphereNonAlloc(origin, abilityRange, _hitResults, data.TargetLayer);
            
            HitboxDebugger.Instance.DrawSphere(origin, abilityRange, Color.darkRed);
            
            Debug.Log($"[DistanceExecutor] Hit Count: {hitCount} | LayerMask: {data.TargetLayer.value} | Origin: {origin} | Range: {abilityRange}");

            for (var i = 0; i < hitCount; i++)
            {
                var hit = _hitResults[i];
                
                if (hit.gameObject == caster) continue;
                
                OnHit(caster, hit.gameObject, data);
            }
        }

        public override void OnHit(GameObject caster, GameObject target, AbilityData data)
        {
            // Calculate damage based on caster's strength and ability's damage multiplier
            //TODO: This GetComponent can be optimized with interface
            var userStats = caster.GetComponent<BaseStats>();
            var baseDamage = userStats?.Strength ?? 10f;
            var abilityPower = data.Power;

            // Apply damage to target
            if (!target.TryGetComponent(out IDamageable damageable)) return;
            
            var damageDealt = damageable.TakeDamage(caster, new BaseStats.DamageData(baseDamage, abilityPower));
            Debug.Log($"[MeleeExecutor] Damaged {target.name} for {damageDealt} (base: {baseDamage}, power: {abilityPower})");
        }
        
        #endregion
    }
}
