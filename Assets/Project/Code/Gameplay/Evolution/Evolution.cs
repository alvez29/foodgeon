using System.Collections.Generic;
using Project.Code.Core.Data.Enums;
using Project.Code.Core.Data.ScriptableObjects;
using UnityEngine;

namespace Project.Code.Gameplay.Evolution
{
    [CreateAssetMenu(fileName = "New Evolution", menuName = "Foodgeon/Evolutions/Evolution")]
    public class Evolution : ScriptableObject
    {
        public struct EnemyTypePrecondition
        {
            public readonly int Amount;
            public readonly EnemyType Type;

            public EnemyTypePrecondition(int amount, EnemyType type)
            {
                Amount = amount;
                Type = type;
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
        
        public readonly List<EnemyTypePrecondition> EnemiesTypePrecondition;
        
        #endregion

        #region Properties

        public AbilityData specialAbility;
        /// <summary>
        /// The level in depth of the evolution in the evolution tree
        /// </summary>
        public int evolutionDepth;
        
        public List<Evolution> possibleNextEvolution = new();


        protected Evolution(List<EnemyTypePrecondition> enemiesTypePrecondition)
        {
            EnemiesTypePrecondition = enemiesTypePrecondition;
        }

        #endregion
    }
}