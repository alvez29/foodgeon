using Project.Code.Core.Data;
using Project.Code.Core.Interfaces;
using Project.Code.Gameplay.Eating.Base;
using Project.Code.Gameplay.Enemies;
using Project.Code.Gameplay.Player.Stats;
using UnityEngine;

namespace Project.Code.Gameplay.Player.Eating
{
    [RequireComponent(typeof(PlayerStats))]
    public class PlayerEatingComponent : BaseEatingComponent
    {
        private void Awake()
        {
            PlayerStats = GetComponent<PlayerStats>();
        }

        public override void PerformEatingAction()
        {
            //TODO: Do the shapecasting and call Eat
        }

        protected override void Eat(GameObject objectToEat)
        {
            if (TryGetComponent(out IEdible edibleComponent))
            {
                //If it is the enemy   
                if (TryGetComponent(out EnemyStats enemyStats))
                {
                    if (!enemyStats.CanBeEaten) return;
                    
                    edibleComponent.OnBeingEaten();
                        
                    PlayerStats.AddDefense(enemyStats.EnemyReward.Defense);
                    PlayerStats.AddSpeed(enemyStats.EnemyReward.Speed);
                    PlayerStats.AddStrength(enemyStats.EnemyReward.Strength);

                    (PlayerStats as PlayerStats)?.AddToBelly(
                        new EatenEnemyData(enemyStats.EnemyName, enemyStats.Flavor));
                }
                else
                {
                    edibleComponent.OnBeingEaten();
                }
                
            }
            else
            {
                Debug.Log("Eww... That is not edible");
            }
        }
    }
}