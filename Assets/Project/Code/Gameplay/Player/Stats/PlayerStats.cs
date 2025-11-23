using System.Collections.Generic;
using Project.Code.Core;
using Project.Code.Core.Data;
using Project.Code.Gameplay.Stats;
using UnityEngine;

namespace Project.Code.Gameplay.Player.Stats
{
    public class PlayerStats : BaseStats
    {
        public event System.Action<int> OnMoneyChanged;
        public event System.Action<EatenEnemyData> OnBellyChanged;
        
        [Header("Player Stats")]
        [SerializeField] private int evolutionStage = 1;
        
        private const int MaxEvolutionStage = 3;

        private int Money { get; set; }
        
        // Belly Storage
        private readonly List<EatenEnemyData> _bellyContents = new();
        public int BellyCount => _bellyContents.Count;
        public IReadOnlyList<EatenEnemyData> BellyContents => _bellyContents;

        private int EvolutionStage
        {
            get => evolutionStage;
            set => evolutionStage = value;
        }

        protected override void Awake()
        {
            SetMaxHealth(Constants.Stats.GetMaxHealthFromEvolution(evolutionStage));
            base.Awake();
        }

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
            if (_bellyContents.Count >= Constants.Stats.MaxBelly)
            {
                Debug.Log("Belly is full!");
                return;
            }
            
            Debug.Log($"Belly: {_bellyContents.Count}/{Constants.Stats.MaxBelly}");
            
            _bellyContents.Add(enemyData);
            OnBellyChanged?.Invoke(enemyData);
        }
    }
}