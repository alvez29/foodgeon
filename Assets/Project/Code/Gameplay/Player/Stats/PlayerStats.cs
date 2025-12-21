using System.Collections.Generic;
using Project.Code.Core;
using Project.Code.Core.Data;
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
        
        #region Serialized Fields
        
        [Header("Player Stats")]
        [SerializeField] private int evolutionStage = 1;
        
        #endregion

        #region Properties

        private int Money { get; set; }
        
        // Belly Storage
        public int BellyCount => _bellyContents.Count;
        public IReadOnlyList<EatenEnemyData> BellyContents => _bellyContents;

        private int EvolutionStage
        {
            get => evolutionStage;
            set => evolutionStage = value;
        }
        
        #endregion

        #region Fields
        
        private readonly List<EatenEnemyData> _bellyContents = new();
        
        #endregion

        #region Unity Functions

        protected override void Awake()
        {
            SetMaxHealth(Constants.Stats.GetMaxHealthFromEvolution(evolutionStage));
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

        public bool AddToBelly(EatenEnemyData enemyData)
        {
            if (_bellyContents.Count >= Constants.Stats.MaxBelly)
            {
                Debug.Log("Belly is full!");
                return false;
            }
            
            Debug.Log($"Belly: {_bellyContents.Count}/{Constants.Stats.MaxBelly}");
            
            _bellyContents.Add(enemyData);
            OnBellyChanged?.Invoke(enemyData);

            return true;
        }
        
        #endregion
    }
}