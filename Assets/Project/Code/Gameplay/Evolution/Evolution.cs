using Project.Code.Core.Data.ScriptableObjects;
using UnityEngine;

namespace Project.Code.Gameplay.Evolution
{
    [CreateAssetMenu(fileName = "New Evolution Data", menuName = "Foodgeon/Evolution/Evolution Data", order = 0)]
    public class Evolution : ScriptableObject
    {
        public string evolutionName;
        
        public float strengthReward;
        public float defenseReward;
        public float speedReward;
        
        public AbilityData specialAbility;
    }
}