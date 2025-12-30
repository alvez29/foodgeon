using System.Collections.Generic;
using Project.Code.Core.Data.Enums;
using Project.Code.Core.Data.ScriptableObjects;
using Project.Code.Gameplay.Player.Stats;
using UnityEngine;

namespace Project.Code.Gameplay.Evolution
{
    [CreateAssetMenu(fileName = "New Evolution", menuName = "Foodgeon/Evolutions/Evolution")]
    public class Evolution : ScriptableObject
    {
        [System.Serializable]
        public struct EnemyTypePrecondition
        {
            public int amount;
            public EnemyType type;

            public EnemyTypePrecondition(int amount, EnemyType type)
            {
                this.amount = amount;
                this.type = type;
            }
        }
        
        #region Evolution Reward
        
        public string evolutionName;
        
        public float strengthReward;
        public float defenseReward;
        public float speedReward;

        #endregion
        
        #region Evolution Precondition

        public float strengthPrecondition;
        public float defensePrecondition;
        public float speedPrecondition;
        
        public List<EnemyTypePrecondition> enemiesTypePrecondition;
        
        #endregion

        #region Properties

        public AbilityData specialAbility;
        /// <summary>
        /// The level in depth of the evolution in the evolution tree
        /// </summary>
        public int evolutionDepth;
        
        public List<Evolution> possibleNextEvolution = new();
        

        #endregion

        #region Public Methods

        public bool SatisfyStatsPreconditions(PlayerStats playerStats)
        {
            return playerStats.Speed >= speedPrecondition && playerStats.Defense >= defensePrecondition &&
                   playerStats.Strength >= strengthPrecondition;
        }

        public bool SatisfyIngredientsPreconditions(PlayerStats playerStats)
        {
            var playerStatsBellyContents = playerStats.BellyContents;
            var evolutionEnemiesTypePrecondition = enemiesTypePrecondition;
               
            //Check for every type conditions
            foreach (var typeCondition in evolutionEnemiesTypePrecondition)
            {
                playerStatsBellyContents.TryGetValue(typeCondition.type, out var bellyTypeCount);

                if (bellyTypeCount < typeCondition.amount)
                {
                    Debug.Log($"[{evolutionName}] Doesn't satisfy type condition." 
                              + $"It was needed {typeCondition.amount} {typeCondition.type}. " 
                              + $"And belly actually has ${bellyTypeCount}. ");
                    return false;
                }
            }

            return true;
        }

        public virtual bool CanEvolve(PlayerStats playerStats)
        {
            if (SatisfyStatsPreconditions(playerStats))
            {
                return SatisfyIngredientsPreconditions(playerStats);
            }

            Debug.Log($"[{evolutionName}] Doesn't satisfy stats condition.");
            return false;
        }
        
        #endregion
    }
}