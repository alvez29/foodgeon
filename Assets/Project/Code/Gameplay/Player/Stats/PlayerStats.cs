using System.Collections.Generic;
using Project.Code.Core;
using Project.Code.Core.Data;
using Project.Code.Core.Data.Enums;
using Project.Code.Gameplay.Stats;
using UnityEngine;

namespace Project.Code.Gameplay.Player.Stats
{
    public class PlayerStats : BaseStats
    {
        #region Constants
        
        private const int MaxEvolutionStage = 3;
        
        #endregion

        #region Events
        
        public event System.Action<int> OnMoneyChanged;
        public event System.Action<EatenEnemyData> OnBellyChanged;
        
        #endregion

        #region Properties

        private int Money { get; set; }
        
        // Belly Storage
        public int BellyCount => BellyContents.Count;
        public Dictionary<EnemyType, int> BellyContents { get; } = new();

        public int evolutionStage = 0;

        #endregion

        #region Unity Functions

        protected override void Awake()
        {
            SetMaxHealth(Constants.Stats.Player.GetMaxHealthFromEvolution(evolutionStage));
            base.Awake();
        }
        
        #endregion

        #region Public Methods

        public void AddMoney(int amount)
        {
            Money += amount;
            OnMoneyChanged?.Invoke(Money);
        }

        public bool SpendMoney(int amount)
        {
            if (Money < amount) return false;
            
            Money -= amount;
            OnMoneyChanged?.Invoke(Money);
            return true;
        }

        public void AddToBelly(EatenEnemyData enemyData)
        {
            if (BellyContents.Count >= Constants.Stats.MaxBelly)
            {
                Debug.Log("Belly is full!");
                return;
            }
            
            Debug.Log($"Belly: {BellyContents.Count}/{Constants.Stats.MaxBelly}");

            BellyContents[enemyData.type] =
                BellyContents.TryGetValue(enemyData.type, out var currentCount) ? currentCount + 1 : 1;
            OnBellyChanged?.Invoke(enemyData);
        }
        
        #endregion
    }
}