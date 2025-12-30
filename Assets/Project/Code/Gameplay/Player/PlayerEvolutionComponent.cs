using System.Linq;
using Project.Code.Core;
using Project.Code.Gameplay.Evolution;
using Project.Code.Gameplay.Player.Stats;
using UnityEngine;

namespace Project.Code.Gameplay.Player
{
    [RequireComponent(typeof(PlayerStats))]
    public class PlayerEvolutionComponent : MonoBehaviour
    {
        [SerializeField] private Evolution.Evolution initialEvolution;
        [SerializeField] private StuffCakeEvolution stuffCakeEvolution;
        [SerializeField] private StuffCakeEvolution superStuffCakeEvolution;

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

        public void TryEvolving()
        {
            // 1. Check if any stat is over threshold to prevent checking every time player eat an enemy
            if (!DoesSatisfyEvolutionStageThreshold(_playerStats))
            {
                Debug.Log("Evolving stage threshold failed");
                return;
            }

            var nextEvolutionList = CurrentEvolution.possibleNextEvolution
                .Where(evolution => evolution.CanEvolve(_playerStats))
                .ToList();

            // 2. If there is exactly one evolution that satisfies all conditions, evolve to it. If there is more than one, choose randomly
            var randomElement = Random.Range(0, nextEvolutionList.Count - 1);
            var nextEvolution = nextEvolutionList.Count >= 1 ? nextEvolutionList[randomElement] : null;

            if (nextEvolution)
            {
                Evolve(nextEvolution);
                _playerStats.EvolutionStage = nextEvolution.evolutionDepth;
                Debug.Log($"Evolved to ${nextEvolution.evolutionName}!!");
            }

            // 3. If not, try to evolve to fallback evolution (stuff cake)
            else
            {
                TryEvolvingToStuffCakeOrDoNothing(CurrentEvolution, _playerStats);
            }
        }

        private static bool DoesSatisfyEvolutionStageThreshold(PlayerStats playerStats)
        {
            var threshold = playerStats.EvolutionStage == 0
                ? Constants.Evolution.FirstDepthStatPrecondition
                : Constants.Evolution.SecondDepthStatPrecondition;

            return playerStats.Defense >= threshold ||
                   playerStats.Speed >= threshold ||
                   playerStats.Strength >= threshold;
        }

        private void TryEvolvingToStuffCakeOrDoNothing(Evolution.Evolution evolution, PlayerStats playerStats)
        {
            var cakeEvolution = evolution.evolutionDepth == 0 ? stuffCakeEvolution : superStuffCakeEvolution;

            if (cakeEvolution.CanEvolve(playerStats))
            {
                Evolve(cakeEvolution);
                Debug.Log("Some stat are not satisfied. Evolved to stuff cake evolution.");
                return;
            }

            Debug.Log("You can evolve into nothing!");
        }

        private void Evolve(Evolution.Evolution evolution)
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