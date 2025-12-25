namespace Project.Code.Core.Data
{
    public class EnemyReward
    {
        public readonly float Strength = 0.0f;
        public readonly float Defense = 0.0f;
        public readonly float Speed = 0.0f;

        public EnemyReward(float strength, float defense, float speed)
        {
            Strength = strength;
            Defense = defense;
            Speed = speed;
        }
    }
}