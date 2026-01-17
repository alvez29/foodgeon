using System;
using System.Collections;
using Project.Code.Core.Data;
using Project.Code.Core.Data.Enums;
using UnityEngine;

namespace Project.Code.Core
{
    public static class Constants
    {
        public static class Movement
        {
            public const float MovementInputThreshold = 0.001f;
            public const float AimInputThreshold = 0.01f;
        }

        public static class Knockback
        {
            public const float KnockbackDuration = 0.05f;
            public const float KnockbackDistance = 1f;
        }
        
        public static class Tags
        {
            public const string Player = "Player";
            public const string Enemy = "Enemy";
        }

        public static class Hit
        {
            public const float PlayerHitStunDuration = 0.2f;
        }
        
        public static class Stats
        {
            public const int MaxBelly = 50;
            public const int MaxEvolutionStage = 3;
            public const float MaxStrength = 60.0f;
            public const float MaxDefense = 60.0f;
            public const float MaxSpeed = 60.0f;
            
            public static class Player
            {
                private const float FirstEvolutionHealth = 10.0f;
                private const float SecondEvolutionHealth = 15.0f;
                private const float ThirdEvolutionHealth = 20.0f;
                
                public const int EatingComboTimes = 3;
                
                public static float GetMaxHealthFromEvolution(int evolutionStage)
                {
                    return evolutionStage switch
                    {
                        1 => FirstEvolutionHealth,
                        2 => SecondEvolutionHealth,
                        3 => ThirdEvolutionHealth,
                        _ => FirstEvolutionHealth
                    };
                }    
            }


            public static class Enemy
            {
                private static readonly EnemyReward SaltyEnemyReward = new(2, 0, 2);
                private static readonly EnemyReward SweetEnemyReward = new(0, 2, 0);
                private static readonly EnemyReward SpicyEnemyReward = new(0, 0, 2);

                public static EnemyReward GetEnemyRewardByFlavour(Flavor flavor)
                {
                    return flavor switch
                    {
                        Flavor.Salty => SaltyEnemyReward,
                        Flavor.Sweet => SweetEnemyReward,
                        Flavor.Spicy => SpicyEnemyReward,
                        _ => new EnemyReward(0, 0, 0)
                    };
                }

                public static Flavor GetFlavorByType(EnemyType type)
                {
                    return type switch
                    {
                        EnemyType.Chocolate => Flavor.Sweet,
                        EnemyType.Yeast => Flavor.Salty,
                        EnemyType.Chilli => Flavor.Spicy,
                        EnemyType.Ginger => Flavor.Spicy,
                        EnemyType.Cheese => Flavor.Salty,
                        EnemyType.Butter => Flavor.Salty,
                        EnemyType.SugarPowder => Flavor.Sweet,
                        EnemyType.Orange => Flavor.Sweet,
                        EnemyType.Bread => Flavor.Salty,
                        EnemyType.Chorizo => Flavor.Spicy,
                        EnemyType.Ham => Flavor.Salty,
                        _ => Flavor.None
                    };
                }
            }
            
            /// <summary>
            /// Centered damage formula
            /// </summary>
            /// <returns>The value of the damage dealt</returns>
            public static float GetDamageValue(float damageAmount, float defense, float abilityPower)
            {
                return Mathf.Clamp(Mathf.Floor((float)(((damageAmount - (defense / 1.2)) / 2.5) + ((abilityPower + 3.0) / 3.0))), 1,
                    10);;
            }
        }

        public static class Evolution
        {
            public const int FirstDepthStatPrecondition = 25;
            public const int SecondDepthStatPrecondition = 50;
        }

        #region Coroutines

        public static class Coroutines
        {
            
            /// <summary>
            /// Simple coroutine for waiting certain amount of time
            /// </summary>
            public static IEnumerator WaitTime(float waitTime, Action onCompleted = null)
            {
                if (waitTime <= 0)
                {
                    onCompleted?.Invoke();
                    yield break;
                }
                
                var elapsedTime = 0f;
            
                while (elapsedTime < waitTime)
                {
                    elapsedTime += Time.deltaTime;

                    if (elapsedTime >= waitTime)
                    {
                        onCompleted?.Invoke();
                    }
                    
                    yield return null;                    
                }
            }

        }
        #endregion
    }
}
