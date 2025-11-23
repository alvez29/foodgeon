using Project.Code.Core.Interfaces;
using Project.Code.Gameplay.Stats;
using UnityEngine;

namespace Project.Code.Gameplay.Eating.Base
{
    [RequireComponent(typeof(BaseStats))]
    public abstract class BaseEdibleComponent : MonoBehaviour, IEdible
    {
        public abstract void OnBeingEaten();
        
    }
}