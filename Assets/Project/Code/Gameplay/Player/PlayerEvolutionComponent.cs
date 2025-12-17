using System;
using Project.Code.Gameplay.Evolution;
using Project.Code.Gameplay.Player.Stats;
using UnityEngine;

namespace Project.Code.Gameplay.Player
{
    [RequireComponent(typeof(PlayerStats))]
    public class PlayerEvolutionComponent : MonoBehaviour
    {
        private Evolution.Evolution _currentEvolution;

        private PlayerStats _playerStats;
        
        private void Awake()
        {
            _playerStats = GetComponent<PlayerStats>();
        }

        public void Evolve(Evolution.Evolution evolution)
        {
            _playerStats.AddDefense(evolution.defenseReward);
            _playerStats.AddSpeed(evolution.speedReward);
            _playerStats.AddStrength(evolution.strengthReward);
            
            _currentEvolution = evolution;
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