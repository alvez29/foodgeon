using System.Diagnostics;
using Project.Code.Core.Data;
using Project.Code.Gameplay.Stats;
using UnityEngine;

namespace Project.Code.Gameplay.Enemies
{
    public class EnemyStats : BaseStats
    {
        [Header("Enemy Stats")]
        [SerializeField] private string enemyName;
        [SerializeField] private Flavor flavor;
        [SerializeField] private EnemyReward reward;
        [SerializeField] private bool edible = false;

        public string EnemyName => enemyName;
        public Flavor Flavor => flavor;

        public void Initialize(int depth)
        {
            var multiplier = 1f + (depth * 0.1f);
            // Logic to scale stats based on depth could go here
        }
    }
}