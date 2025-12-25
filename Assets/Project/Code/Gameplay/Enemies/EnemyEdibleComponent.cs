using Project.Code.Gameplay.Eating.Base;
using UnityEngine;

namespace Project.Code.Gameplay.Enemies
{
    public class EnemyEdibleComponent : BaseEdibleComponent
    {
        public override void OnBeingEaten()
        {
            Debug.Log($"[EnemyEdibleComponent] Enemy has been eaten!");
            Destroy(gameObject);
        }
    }
}