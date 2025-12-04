using UnityEngine;

namespace Project.Code.Gameplay.Enemies
{
    public abstract class BaseEnemyMovement : MonoBehaviour
    {
        public abstract void Move(Vector3 direction);
    }
}