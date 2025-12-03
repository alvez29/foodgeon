using System;

namespace Project.Code.Core
{
    public static class Constants
    {
        public static class Movement
        {
            public const float MovementInputThreshold = 0.001f;
            public const float AimInputThreshold = 0.01f;
        }
        
        public static class Tags
        {
            public const string Player = "Player";
            public const string Enemy = "Enemy";
        }
        
        public static class Stats
        {
            public const int MaxBelly = 50;
            public const int MaxEvolutionStage = 3;
            public const float MaxStrength = 60.0f;
            public const float MaxDefense = 60.0f;
            public const float MaxSpeed = 60.0f;
            
            private const float FirstEvolutionHealth = 10.0f;
            private const float SecondEvolutionHealth = 15.0f;
            private const float ThirdEvolutionHealth = 20.0f;
            
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
    }
}
