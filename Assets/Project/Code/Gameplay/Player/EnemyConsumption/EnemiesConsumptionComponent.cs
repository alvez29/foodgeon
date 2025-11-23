using System;
using Project.Code.Core.Data;
using Project.Code.Gameplay.Enemies;
using Project.Code.Gameplay.Player.Stats;
using UnityEngine;

namespace Project.Code.Gameplay.Player.EnemyConsumption
{
    [RequireComponent(typeof(PlayerStats))]
    public class EnemiesConsumptionComponent : MonoBehaviour
    {
        public event Action<EatenEnemyData> OnEnemyEaten;
        private PlayerStats _playerStats;

        private void Awake()
        {
            _playerStats = GetComponent<PlayerStats>();
        }
        
        public void EatEnemy(EnemyStats enemyStats)
        {
            var enemyEatenData = new EatenEnemyData(enemyStats.EnemyName, enemyStats.Flavor);
            
            OnEnemyEaten?.Invoke(enemyEatenData);
            
            _playerStats.AddDefense(enemyStats.Defense);
            _playerStats.AddSpeed(enemyStats.Speed);
            _playerStats.AddStrength(enemyStats.Strength);
            
            _playerStats.AddToBelly(enemyEatenData);

            Debug.Log($"[PlayerStats] Ate {enemyEatenData.name} ({enemyEatenData.flavor}).");
        }
    }
}