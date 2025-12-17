using UnityEngine;

namespace Project.Code.Core.Interfaces
{
    public interface IDamageable
    {
        float TakeDamage(float amount, float abilityPower, GameObject source);
    }
}
