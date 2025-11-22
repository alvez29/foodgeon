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
        [Header("Scratch Settings")]
        [SerializeField] private float range = 1.5f;
        [SerializeField] private float angle = 90f;
        [SerializeField] private LayerMask targetLayer;

        private readonly Collider[] _hitResults = new Collider[10];

        public override void Activate(GameObject subject)
        {
            var userStats = subject.GetComponent<BaseStats>();
            var damage = userStats != null ? userStats.Strength : 10f;

            var origin = subject.transform.position;
            var forward = subject.transform.forward;

            HitboxDebugger.Instance.DrawSphere(origin, range, Color.red, 0.5f);

            var hitCount = Physics.OverlapSphereNonAlloc(origin, range, _hitResults, targetLayer);
            
            Debug.Log($"[ScratchAbility] Hit Count: {hitCount} | LayerMask: {targetLayer.value} | Origin: {origin}");

            for (var i = 0; i < hitCount; i++)
            {
                var hit = _hitResults[i];
                
                if (hit.gameObject == subject) continue;

                var directionToTarget = (hit.transform.position - origin).normalized;

                if (!(Vector3.Angle(forward, directionToTarget) < angle / 2)) continue;
                
                if (hit.TryGetComponent(out IDamageable damageable))
                {
                    damageable.TakeDamage(damage, subject);
                    Debug.Log($"[ScratchAbility] Damaged {hit.name} for {damage}");
                }
            }
        }
    }
}
