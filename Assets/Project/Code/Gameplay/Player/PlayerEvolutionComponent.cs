using Project.Code.Gameplay.Player.Stats;
using UnityEngine;

namespace Project.Code.Gameplay.Player
{
    [RequireComponent(typeof(PlayerStats))]
    public class PlayerEvolutionComponent : MonoBehaviour
    {
        [SerializeField] private Evolution.Evolution initialEvolution;
        
        public Evolution.Evolution CurrentEvolution { get; private set; }

        private PlayerStats _playerStats;

        #region Unity Functions

        private void Awake()
        {
            _playerStats = GetComponent<PlayerStats>();

            CurrentEvolution = initialEvolution;
        }

        #endregion
        
        #region Public Functions
        
        public void Evolve(Evolution.Evolution evolution)
        {
            _playerStats.AddDefense(evolution.defenseReward);
            _playerStats.AddSpeed(evolution.speedReward);
            _playerStats.AddStrength(evolution.strengthReward);
            
            CurrentEvolution = evolution;
        }

        public void UseSpecialAbility()
        {
            CurrentEvolution.specialAbility.Use(gameObject);
        }
        
        public float GetCurrentEvolutionCooldown()
        {
            return CurrentEvolution.specialAbility.Cooldown;
        }
        
        #endregion
    }
}