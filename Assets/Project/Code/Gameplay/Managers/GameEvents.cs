using System;
using UnityEngine;

namespace Project.Code.Gameplay.Managers
{
    public static class GameEvents
    {
        public static event Action<float, float, float, GameObject> OnAnyDamageTaken;

        public static void RaiseDamage(float currentHealth, float maxHealth, float damageTaken, GameObject source)
        {
            OnAnyDamageTaken?.Invoke(currentHealth, maxHealth, damageTaken, source);
        }
    }
}