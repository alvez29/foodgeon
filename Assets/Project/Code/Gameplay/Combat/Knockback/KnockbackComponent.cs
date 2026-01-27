using System.Collections;
using Project.Code.Core;
using Project.Code.Gameplay.Stats;
using UnityEngine;
using UnityEngine.AI;

namespace Project.Code.Gameplay.Combat.Knockback
{
    [RequireComponent(typeof(BaseStats))]
    public class KnockbackComponent : MonoBehaviour
    {
        [SerializeField]
        private float knockbackDuration = Constants.Knockback.KnockbackDuration;
        [SerializeField] 
        private float knockbackDistance = Constants.Knockback.KnockbackDistance;
        [SerializeField]
        private AnimationCurve knockbackCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

        private BaseStats _baseStats;
        private CharacterController _characterController;
        private NavMeshAgent _navMeshAgent;

        private void Awake()
        {
            _baseStats = GetComponent<BaseStats>();
            _characterController = GetComponent<CharacterController>();
            _navMeshAgent = GetComponent<NavMeshAgent>();
        }

        private void OnEnable()
        {
            if (_baseStats)
            {
                _baseStats.OnDamageTaken += BaseStatsOnDamageTaken;    
            }
        }

        private void OnDisable()
        {
            if (_baseStats)
            {
                _baseStats.OnDamageTaken -= BaseStatsOnDamageTaken;    
            }
        }

        private void BaseStatsOnDamageTaken(float currentHealth, float maxHealth, float amount, GameObject source)
        {
            // If we don't have a mover component, we can't be knocked back safely
            if (_characterController == null && _navMeshAgent == null) return;
            
            var knockbackDirection = transform.position - source.transform.position;
            // Flatten direction to avoid flying up/down
            knockbackDirection.y = 0;
            knockbackDirection.Normalize();
            
            StartCoroutine(KnockbackCoroutine(knockbackDirection));   
        }

        private IEnumerator KnockbackCoroutine(Vector3 knockbackDirection)
        {
            var elapsedTime = 0f;
            
            // For NavMeshAgent, we want to disable auto-movement/pathing temporarily if needed, 
            // but agent.Move() generally works alongside it. 
            // Sometimes it's better to disable current path or set isStopped=true if the knockback is strong.
            // For now, we just apply the Move.
            
            while (elapsedTime < knockbackDuration)
            {
                var t = elapsedTime / knockbackDuration;
                var curveValue = knockbackCurve.Evaluate(t);
                
                // Note: The original formula: dashSpeed = (distance / duration) * curveValue
                // This implies 'curveValue * constant_speed'. 
                // If the curve goes 0->1, this moves faster at the end? 
                // Usually an ease-out (fast start, slow end) is better for knockback.
                // Assuming the user is happy with the existing curve logic, we keep it.
                
                var dashSpeed = (knockbackDistance / knockbackDuration) * curveValue;
                var motion = knockbackDirection * (dashSpeed * Time.deltaTime);
                
                if (_characterController)
                {
                    _characterController.Move(motion);
                }
                else if (_navMeshAgent)
                {
                    _navMeshAgent.Move(motion);
                }

                elapsedTime += Time.deltaTime;
                yield return null;
            }
        }
    }
}