using Project.Code.Core;
using Project.Code.Gameplay.Stats;
using UnityEngine;

namespace Project.Code.Gameplay.Player
{
    public class PlayerStats : BaseStats
    {
        public event System.Action<int> OnMoneyChanged;
        public event System.Action<int, int> OnBellyChanged;
        
        [Header("Player Stats")]
        [SerializeField] private int evolutionStage = 1;
        
        private const int MaxEvolutionStage = 3;

        private int Money { get; set; }
        private int Belly { get; set; }
        private int EvolutionStage
        {
            get => evolutionStage;
            set => evolutionStage = value;
        }

        protected override void Awake()
        {
            SetMaxHealth(Constants.Stats.GetMaxHealthFromEvolution(evolutionStage));
            
            base.Awake();
            
            Belly = 0;
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
        
        public void AddBelly(int amount)
        {
            Belly = Mathf.Clamp(Belly + amount, 0, Constants.Stats.MaxBelly);
            OnBellyChanged?.Invoke(Belly, Constants.Stats.MaxBelly);
        }

        public void ReduceBelly(int amount)
        {
            Belly = Mathf.Clamp(Belly - amount, 0, Constants.Stats.MaxBelly);
            OnBellyChanged?.Invoke(Belly, Constants.Stats.MaxBelly);
        }
    }
}