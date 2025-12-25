using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Project.Code.Core;
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
        private Evolution.Evolution _stuffCakeEvolution;
        private Evolution.Evolution _superStuffCakeEvolution;

        #region Unity Functions

        private void Awake()
        {
            _playerStats = GetComponent<PlayerStats>();
            _stuffCakeEvolution =
                Resources.Load<Evolution.Evolution>("Assets/Project/Code/Gameplay/Evolution/StuffCakeEvolution.asset");
            _superStuffCakeEvolution =
                Resources.Load<Evolution.Evolution>(
                    "Assets/Project/Code/Gameplay/Evolution/SuperStuffCakeEvolution.asset");

            CurrentEvolution = initialEvolution;
        }

        #endregion

        #region Public Functions

        public void TryEvolving()
        {
            var nextEvolutionList = CurrentEvolution.possibleNextEvolution
                .Where(evolution => SatisfyPrecondition(evolution, _playerStats))
                .ToList();

            // If there is exactly one evolution that satisfies all conditions, evolve to it.
            var nextEvolution = nextEvolutionList.Count == 1 ? nextEvolutionList.First() : null;

            if (nextEvolution)
            {
                Evolve(nextEvolution);
                Debug.Log($"Evolved to ${nextEvolution.evolutionName}!!");
            }
            // If not, try to evolve to stuff cake
            else
            {
                TryEvolvingToStuffCakeOrDoNothing(CurrentEvolution, _playerStats);
            }
        }

        private static bool SatisfyPrecondition(Evolution.Evolution evolution, PlayerStats playerStats)
        {
            // Does player satisfy stats preconditions?
            if (DoesSatisfyStatsPrecondition(evolution, playerStats))
            {
                var playerStatsBellyContents = playerStats.BellyContents;

                //Check for every type conditions
                foreach (var typeCondition in evolution.EnemiesTypePrecondition)
                {
                    playerStatsBellyContents.TryGetValue(typeCondition.Type, out var bellyTypeCount);

                    if (bellyTypeCount < typeCondition.Amount)
                    {
                        Debug.Log($"[{evolution.evolutionName}] Doesn't satisfy type condition." 
                                  + $"It was needed ${typeCondition.Amount} ${typeCondition.Type}. " 
                                  + $"And belly actually has ${bellyTypeCount}. ");
                        return false;
                    }
                }

                return true;
            }

            Debug.Log($"[{evolution.evolutionName}] Doesn't satisfy stats condition.");
            return false;
        }

        private void TryEvolvingToStuffCakeOrDoNothing(Evolution.Evolution currentEvolution, PlayerStats playerStats)
        {
            switch (currentEvolution.evolutionDepth)
            {
                case 0:
                    if (playerStats.Defense > Constants.Evolution.StuffCakeEvolutionStatPrecondition ||
                        playerStats.Speed > Constants.Evolution.StuffCakeEvolutionStatPrecondition ||
                        playerStats.Strength > Constants.Evolution.StuffCakeEvolutionStatPrecondition)
                    {
                        Evolve(_stuffCakeEvolution);
                        Debug.Log("Some stat are 25. Evolved to stuff cake evolution.");
                    }
                    return;
                case 1:
                    if (playerStats.Defense > Constants.Evolution.SuperStuffCakeEvolutionStatPrecondition ||
                        playerStats.Speed > Constants.Evolution.SuperStuffCakeEvolutionStatPrecondition ||
                        playerStats.Strength > Constants.Evolution.SuperStuffCakeEvolutionStatPrecondition)
                    {
                        Evolve(_superStuffCakeEvolution);
                        Debug.Log("Some stat are 50. Evolved to super stuff cake evolution.");
                    }
                    return;
            }
            
            Debug.Log("You can evolve into nothing!");
        }
        
        private static bool DoesSatisfyStatsPrecondition([CanBeNull] Evolution.Evolution evolution,
            PlayerStats playerStats)
        {
            return evolution && playerStats.Speed >= evolution.speedPrecondition &&
                   playerStats.Defense >= evolution.defensePrecondition &&
                   playerStats.Strength >= evolution.strengthPrecondition;
        }   

        private void Evolve(Evolution.Evolution evolution)
        {
            _playerStats.AddDefense(evolution.defenseReward);
            _playerStats.AddSpeed(evolution.speedReward);
            _playerStats.AddStrength(evolution.strengthReward);

            CurrentEvolution = evolution;
        }

        private void ShouldEvolveToStuffCake()
        {
            
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