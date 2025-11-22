using System;
using Project.Code.Core.Interfaces;
using Project.Code.Gameplay.Combat.Abilities.Base;
using Project.Code.Gameplay.Stats;
using Project.Code.Utils;
using UnityEngine;

namespace Project.Code.Gameplay.Combat.Abilities.PrimaryAbilities
{
    [CreateAssetMenu(fileName = "New Scratch Ability", menuName = "Foodgeon/Abilities/Scratch Ability")]
    public class ScratchAbility : Ability
    {
        [Header("Melee Settings")]
        [SerializeField] private float damageMultiplier = 1f;
        [SerializeField] private float range = 1.5f;
        [SerializeField] private float angle = 90f;
        [SerializeField] private LayerMask targetLayer;

        public override void Activate(GameObject subject)
        {
            // 1. Get User Stats (for damage calculation)
            var userStats = subject.GetComponent<BaseStats>();
            float baseDamage = userStats != null ? userStats.Strength : 10f;
            float finalDamage = baseDamage * damageMultiplier;

            // 2. Find Targets (OverlapSphere + Angle Check)
            var origin = subject.transform.position;
            var forward = subject.transform.forward;
            var results = Array.Empty<Collider>();

            var size = Physics.OverlapSphereNonAlloc(origin, range, results, targetLayer);
            
            // Debug Visualization
            HitboxDebugger.Instance.DrawSphere(origin, range, Color.red, 0.5f);

            foreach (var hit in results)
            {
                // Skip self
                if (hit.gameObject == subject) continue;

                var directionToTarget = (hit.transform.position - origin).normalized;
                var isTargetInRange = Vector3.Angle(forward, directionToTarget) < angle / 2;
                
                if (isTargetInRange)
                {
                    // 3. Apply Damage
                    var damageable = hit.GetComponent<IDamageable>();
                    if (damageable != null)
                    {
                        damageable.TakeDamage(finalDamage, subject);
                        Debug.Log($"Hit {hit.name} for {finalDamage} damage!");
                    }
                }
            }
        }
    }
}
