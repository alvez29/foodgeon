using UnityEngine;

namespace Project.Code.Core.Data.ScriptableObjects
{
    [CreateAssetMenu(fileName = "New Enemy Reward", menuName = "Foodgeon/Reward/Enemy Reward", order = 0)]
    public class EnemyReward : ScriptableObject
    {
        
        [SerializeField] public float strength = 0.0f;
        [SerializeField] public float defense = 0.0f;
        [SerializeField] public float speed  = 0.0f;
        
    }
}