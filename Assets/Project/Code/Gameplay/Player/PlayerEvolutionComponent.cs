using System;
using Project.Code.Gameplay.Evolution;
using Project.Code.Gameplay.Player.Stats;
using UnityEngine;

namespace Project.Code.Gameplay.Player
{
    [RequireComponent(typeof(PlayerStats))]
    public class PlayerEvolutionComponent : MonoBehaviour
    {
        private EvolutionData _currentEvolution;

        private PlayerStats _playerStats;
        
        private void Awake()
        {
            _playerStats = GetComponent<PlayerStats>();
        }

        public void Evolve(EvolutionData evolutionData)
        {
            _playerStats.AddDefense(evolutionData.defenseReward);
            _playerStats.AddSpeed(evolutionData.speedReward);
            _playerStats.AddStrength(evolutionData.strengthReward);
            
            _currentEvolution = evolutionData;
        }

        public void UseSpecialAbility()
        {
            _currentEvolution.specialAbility.Use(gameObject);
        }

        public float GetCurrentEvolutionCooldown()
        {
            return _currentEvolution.specialAbility.Cooldown;
        }
    }
}