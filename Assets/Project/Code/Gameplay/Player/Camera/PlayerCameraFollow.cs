using Project.Code.Gameplay.Camera;
using UnityEngine;

namespace Project.Code.Gameplay.Player.Camera
{
    public class PlayerCameraFollow : CameraFollow
    {
        [SerializeField] [Header("Target Settings")]
        private PlayerAimComponent playerAimComponent;

        [SerializeField] private float maxAimVectorLength = 0.1f;

        protected override void FollowTarget(float deltaTime)
        {
            if (!playerAimComponent || !target) return;


            var targetPosition = target.position +
                                 new Vector3(playerAimComponent.AimDirection.x, 0, playerAimComponent.AimDirection.y) *
                                 maxAimVectorLength
                                 + offset;


            transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed * deltaTime);
        }
    }
}