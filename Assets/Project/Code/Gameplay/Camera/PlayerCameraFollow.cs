using System;
using Project.Code.Gameplay.Player;
using UnityEngine;

namespace Project.Code.Gameplay.Camera
{
    public class PlayerCameraFollow : CameraFollow
    {
        [SerializeField] [Header("Target Settings")]
        private PlayerInputHandler playerInputHandler;

        [SerializeField] private float maxAimVectorLength = 0.1f;

        protected override void FollowTarget(float deltaTime)
        {
            if (!playerInputHandler || !target) return;


            var targetPosition = target.position +
                                 new Vector3(playerInputHandler.AimInput.x, 0, playerInputHandler.AimInput.y) *
                                 maxAimVectorLength
                                 + offset;


            transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed * deltaTime);
        }
    }
}