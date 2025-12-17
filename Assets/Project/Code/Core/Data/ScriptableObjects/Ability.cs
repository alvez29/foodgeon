using UnityEngine;

namespace Project.Code.Core.Data.ScriptableObjects
{
    [CreateAssetMenu(fileName = "New Ability", menuName = "Foodgeon/Abilities/Ability Data")]
    public class AbilityData : ScriptableObject
    {
        [Header("Ability Settings")]
        [SerializeField] private string abilityName;
        [SerializeField] private float cooldown = 1f;
        
        [Header("Execution")]
        [SerializeField] private AbilityExecutor executor;
        
        [Header("Parameters")]
        [SerializeField] private float range = 1.5f;
        [SerializeField] private float angle = 90f;
        [SerializeField] private LayerMask targetLayer;
        [SerializeField] private float power = 1f;

        public string AbilityName => abilityName;
        public float Cooldown => cooldown;
        public AbilityExecutor Executor => executor;
        public float Range => range;
        public float Angle => angle;
        public LayerMask TargetLayer => targetLayer;
        public float Power => power;
        
        public void Use(GameObject subject)
        {
            if (executor == null)
            {
                Debug.LogError($"[AbilityData] No executor assigned for ability '{abilityName}'");
                return;
            }
            
            executor.Execute(subject, this);
        }
        
        public void OnHit(GameObject subject, GameObject hitObject)
        {
            if (executor == null)
            {
                Debug.LogError($"[AbilityData] No executor assigned for ability '{abilityName}'");
                return;
            }
            
            executor.OnHit(subject, hitObject, this);
        }
    }
}
