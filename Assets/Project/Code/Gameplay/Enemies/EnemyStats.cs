using Project.Code.Core.Data;
using Project.Code.Core.Data.ScriptableObjects;
using Project.Code.Core.Interfaces;
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
        [SerializeField] private bool isEdible = false;

        public string EnemyName => enemyName;
        public Flavor Flavor => flavor;
        public EnemyReward EnemyReward => reward;
        
        //This can be in the StateComponent
        public bool CanBeEaten => isEdible;

        public void Initialize(int depth)
        {
            var multiplier = 1f + (depth * 0.1f);
            // Logic to scale stats based on depth could go here
        }
    }
}