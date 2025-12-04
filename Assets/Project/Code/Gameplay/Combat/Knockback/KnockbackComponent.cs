using System.Collections;
using Project.Code.Core;
using Project.Code.Gameplay.Stats;
using UnityEngine;

namespace Project.Code.Gameplay.Combat.Knockback
{
    [RequireComponent(typeof(BaseStats))]
    [RequireComponent(typeof(CharacterController))]
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

        private void Awake()
        {
            _baseStats = GetComponent<BaseStats>();
            _characterController = GetComponent<CharacterController>();
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
            var knockbackDirection = transform.position - source.transform.position;
            StartCoroutine(KnockbackCoroutine(knockbackDirection));   
        }

        private IEnumerator KnockbackCoroutine(Vector3 knockbackDirection)
        {
            var elapsedTime = 0f;
            
            while (elapsedTime < knockbackDuration)
            {
                var t = elapsedTime / knockbackDuration;
                var curveValue = knockbackCurve.Evaluate(t);
                var dashSpeed = (knockbackDistance / knockbackDuration) * curveValue;

                _characterController.Move(knockbackDirection * (dashSpeed * Time.deltaTime));

                elapsedTime += Time.deltaTime;
                yield return null;
            }
        }
    }
}