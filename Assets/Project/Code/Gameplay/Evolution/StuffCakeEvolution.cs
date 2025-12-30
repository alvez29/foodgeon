using Project.Code.Gameplay.Player.Stats;
using UnityEngine;

namespace Project.Code.Gameplay.Evolution
{
    [CreateAssetMenu(fileName = "New Evolution", menuName = "Foodgeon/Evolutions/Stuff Cake Evolution")]
    public class StuffCakeEvolution: Evolution
    {
        public int statPrecondition;

        public StuffCakeEvolution InitPrecondition(int newStatPrecondition)
        {
            statPrecondition = newStatPrecondition;
            return this;
        }
        
        public override bool CanEvolve(PlayerStats playerStats)
        {
            return playerStats.Defense > statPrecondition || playerStats.Speed > statPrecondition || playerStats.Strength > statPrecondition;
        }
    }
}