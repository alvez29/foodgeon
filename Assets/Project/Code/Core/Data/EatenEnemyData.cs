using System;
using Project.Code.Core.Data.Enums;

namespace Project.Code.Core.Data
{
    [System.Serializable]
    public struct EatenEnemyData : IEquatable<EatenEnemyData>
    {
        public EnemyType type;
        public Flavor flavor;

        public EatenEnemyData(EnemyType type , Flavor flavor)
        {
            this.type = type;
            this.flavor = flavor;
        }

        public bool Equals(EatenEnemyData other)
        {
            return type == other.type && flavor == other.flavor;
        }

        public override bool Equals(object obj)
        {
            return obj is EatenEnemyData other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine((int)type, (int)flavor);
        }
    }
}