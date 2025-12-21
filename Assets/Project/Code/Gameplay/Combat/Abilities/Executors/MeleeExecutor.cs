using Project.Code.Core.Data.ScriptableObjects;
using Project.Code.Core.Interfaces;
using Project.Code.Gameplay.Stats;
using Project.Code.Utils;
using UnityEngine;

namespace Project.Code.Gameplay.Combat.Abilities.Executors
{
    /// <summary>
    /// Executor for melee cone-based attacks.
    /// Detects targets in a sphere around the caster and filters by cone angle.
    /// </summary>
    [CreateAssetMenu(fileName = "New Melee Executor", menuName = "Foodgeon/Executors/Melee Executor")]
    public class MeleeExecutor : AbilityExecutor
    {
        #region Fields
        
        private readonly Collider[] _hitResults = new Collider[10];
        
        #endregion

        #region Override Methods

        public override void Execute(GameObject caster, AbilityData data)
        {
            var origin = caster.transform.position;
            var forward = caster.transform.forward;

            // Debug visualization
            HitboxDebugger.Instance.DrawSphere(origin, data.Range, Color.red, 0.5f);

            // Detect all colliders in range
            var hitCount = Physics.OverlapSphereNonAlloc(origin, data.Range, _hitResults, data.TargetLayer);
            
            Debug.Log($"[MeleeExecutor] Hit Count: {hitCount} | LayerMask: {data.TargetLayer.value} | Origin: {origin}");

            // Filter by cone angle
            for (var i = 0; i < hitCount; i++)
            {
                var hit = _hitResults[i];
                
                // Skip self
                if (hit.gameObject == caster) continue;

                // Check if target is within cone angle
                var directionToTarget = (hit.transform.position - origin).normalized;

                if (!(Vector3.Angle(forward, directionToTarget) < data.Angle / 2)) continue;
                
                OnHit(caster, hit.gameObject, data);
            }
        }

        public override void OnHit(GameObject caster, GameObject target, AbilityData data)
        {
            // Calculate damage based on caster's strength and ability's damage multiplier
            var userStats = caster.GetComponent<BaseStats>();
            var baseDamage = userStats?.Strength ?? 10f;
            var abilityPower = data.Power;

            // Apply damage to target
            if (!target.TryGetComponent(out IDamageable damageable)) return;
            
            var damageDealt = damageable.TakeDamage(baseDamage, data.Power, caster);
            Debug.Log($"[MeleeExecutor] Damaged {target.name} for {damageDealt} (base: {baseDamage}, power: {abilityPower})");
        }
        
        #endregion
    }
}
