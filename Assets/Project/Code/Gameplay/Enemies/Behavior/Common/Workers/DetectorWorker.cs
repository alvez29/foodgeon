using UnityEngine;

namespace Project.Code.Gameplay.Enemies.Behavior.Common.Workers
{
    public class DetectorWorker : MonoBehaviour
    {
        public float radius;
        public LayerMask layerMask;
        
        public GameObject TryDetectingObject()
        {
            var colliders = new Collider[1];
            Physics.OverlapSphereNonAlloc(transform.position, radius, colliders,layerMask);
            // ReSharper disable once Unity.PreferNonAllocApi
            return colliders[0]?.gameObject;
        }
    }
}
