using Project.Code.Gameplay.Player;
using Project.Code.Gameplay.Stats;
using UnityEngine;

namespace Project.Code.Gameplay.Eating.Base
{
    [RequireComponent(typeof(BaseStats))]
    public abstract class BaseEatingComponent : MonoBehaviour
    {
        [SerializeField] protected float eatingRange = 5.0f;
        [SerializeField] protected LayerMask targetLayer;
        
        protected internal bool IsEating = false;
        
        protected BaseStats PlayerStats;
    
        public abstract void PerformEatingAction();

        protected abstract bool TryEating(GameObject objectToEat);
    }
}