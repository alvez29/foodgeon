using Project.Code.Gameplay.Stats;
using UnityEngine;

namespace Project.Code.Gameplay.Eating.Base
{
    [RequireComponent(typeof(BaseStats))]
    public abstract class BaseEatingComponent : MonoBehaviour
    {
        protected BaseStats PlayerStats;

        public abstract void PerformEatingAction();

        protected abstract void Eat(GameObject objectToEat);
    }
}