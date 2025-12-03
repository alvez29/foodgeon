using Project.Code.Gameplay.Combat.Abilities.Base;
using UnityEngine;

namespace Project.Code.Gameplay.Evolution
{
    [CreateAssetMenu(fileName = "New Evolution Data", menuName = "Foodgeon/Evolution/Evolution Data", order = 0)]
    public class EvolutionData : ScriptableObject
    {
        public string evolutionName;
        
        public float strengthReward;
        public float defenseReward;
        public float speedReward;
        
        public Ability specialAbility;
    }
}