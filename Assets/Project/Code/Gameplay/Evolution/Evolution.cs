using System.Collections.Generic;
using Project.Code.Core.Data.Enums;
using Project.Code.Core.Data.ScriptableObjects;
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
    }
}