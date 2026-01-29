using Project.Code.Gameplay.Stats;
using UnityEngine;

namespace Project.Code.Core.Interfaces
{
    public interface IDamageable
    {
        float TakeDamage(GameObject source, BaseStats.DamageData damageData);
    }
}
