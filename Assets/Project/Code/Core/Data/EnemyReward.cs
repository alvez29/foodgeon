using UnityEngine;

namespace Project.Code.Core.Data
{
    [CreateAssetMenu(fileName = "New Enemy Reward", menuName = "Foodgeon/Reward/Enemy Reward", order = 0)]
    public class EnemyReward : ScriptableObject
    {
        private float Strength { get; set; } = 0.0f;
        private float Defense { get; set; } = 0.0f;
        private float Speed { get; set; } = 0.0f;

        public EnemyReward(float strength, float defense, float speed)
        {
            Strength = strength;
            Defense = defense;
            Speed = speed;
        }
    }
}