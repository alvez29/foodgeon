using System;
using DG.Tweening;
using JetBrains.Annotations;
using Project.Code.Gameplay.Camera;
using UnityEngine;

namespace Project.Code.Gameplay.Player.Camera
{
    public class PlayerCameraZoom : MonoBehaviour
    {
        [CanBeNull] [SerializeField] 
        private PlayerInputHandler inputHandler;
        [CanBeNull] [SerializeField]
        private CameraFollow cameraFollow;
        [SerializeField] private Vector3 zoomOutCameraOffset;
        [SerializeField] private float zoomOutDuration = 0.5f;
        
        private Vector3 _initialCameraOffset = Vector2.zero;

        private void Start()
        {
            _initialCameraOffset = cameraFollow?.offset ?? Vector3.down;
            print($"Offset: {_initialCameraOffset}");
        }

        private void OnEnable()
        {
            if (inputHandler != null)
                inputHandler.OnZoomStopped += () =>
                {
                    if (cameraFollow)
                    {
                        
                        DOTween.To(() => cameraFollow.offset, x => cameraFollow.offset = x, _initialCameraOffset, zoomOutDuration).SetEase(Ease.OutExpo);    
                    }
                };

            if (inputHandler != null)
                inputHandler.OnZoomStarted += () =>
                {
                    if (cameraFollow)
                    {
                        DOTween.To(() => cameraFollow.offset, x => cameraFollow.offset = x, zoomOutCameraOffset, zoomOutDuration).SetEase(Ease.OutExpo);    
                    }    
                };
        }
    }
}
