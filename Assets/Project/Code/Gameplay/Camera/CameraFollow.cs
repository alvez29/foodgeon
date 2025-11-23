using UnityEngine;

namespace Project.Code.Gameplay.Camera
{
    public class CameraFollow : MonoBehaviour
    {
        [Header("Target Settings")]
        [SerializeField] private Transform target;
        [SerializeField] private Vector3 offset = new Vector3(0f, 8f, -10f);
        
        [Header("Movement Settings")]
        [SerializeField] private float smoothSpeed = 5f;
        [SerializeField] private bool useFixedUpdate = true;

        private Vector3 _currentVelocity;

        public void SetTarget(Transform newTarget)
        {
            target = newTarget;
        }

        private void Start()
        {
            if (target != null) return;
            
            var player = FindAnyObjectByType<Project.Code.Gameplay.Player.PlayerInputHandler>();
            
            if (player != null)
            {
                SetTarget(player.transform);
            }
        }

        private void LateUpdate()
        {
            if (!useFixedUpdate)
            {
                FollowTarget(Time.deltaTime);
            }
        }

        private void FixedUpdate()
        {
            if (useFixedUpdate)
            {
                FollowTarget(Time.fixedDeltaTime);
            }
        }

        private void FollowTarget(float deltaTime)
        {
            if (!target) return;

            var targetPosition = target.position + offset;
            transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed * deltaTime);
        }
        
        private void OnValidate()
        {
            if (target == null || Application.isPlaying) return;
            
            transform.position = target.position + offset;
            transform.LookAt(target);
        }
    }
}
