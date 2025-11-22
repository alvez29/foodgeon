using Project.Code.Core.Data;
using Project.Code.Gameplay.Stats;
using UnityEngine;
using UnityEngine.Serialization;

namespace Project.Code.Gameplay.Enemies
{
    public class EnemyStats : BaseStats
    {
        [Header("Enemy Stats")]
        [SerializeField] private Flavor flavor;
        [SerializeField] private EnemyReward reward;

        public Flavor Flavor => flavor;
        public EnemyReward Reward => reward;

        public void Initialize(int depth)
        {
            var multiplier = 1f + (depth * 0.1f);
        }
    }
}