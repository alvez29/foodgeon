using UnityEngine;

namespace Project.Code.Core.Interfaces
{
    public interface IDamageable
    {
        void TakeDamage(float amount, GameObject source);
    }
}
