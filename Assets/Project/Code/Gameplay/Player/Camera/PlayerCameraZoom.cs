using System;
using DG.Tweening;
using JetBrains.Annotations;
using Project.Code.Gameplay.Camera;
using UnityEngine;

namespace Project.Code.Gameplay.Player.Camera
{
    public class PlayerCameraZoom : MonoBehaviour
    {
        public event Action OnZoomCompleted;
        
        [CanBeNull] [SerializeField] 
        private PlayerInputHandler inputHandler;
        [CanBeNull] [SerializeField]
        private CameraFollow cameraFollow;
        [SerializeField] private Vector3 zoomOutCameraOffset;
        [SerializeField] private float zoomOutDuration = 0.5f;

        
        private Vector3 _initialCameraOffset = Vector2.zero;
        private Action _resetZoomOutAction;
        
        #region Unity events
        private void Start()
        {
            _initialCameraOffset = cameraFollow?.offset ?? Vector3.down;
        }

        private void OnEnable()
        {
            if (inputHandler == null) return;
            
            inputHandler.OnZoomStarted += DoZoomOut;
            _resetZoomOutAction = () => ResetZoom(zoomOutDuration);
            inputHandler.OnZoomStopped += _resetZoomOutAction;
        }

        private void OnDisable()
        {
            if (inputHandler == null) return;
            
            inputHandler.OnZoomStarted -= DoZoomOut;
            inputHandler.OnZoomStopped += _resetZoomOutAction;
            _resetZoomOutAction = null;
        }
        #endregion
        
        #region Public methods

        public void ResetZoom(float duration = 0.2f)
        {
            DoCustomZoom(_initialCameraOffset, zoomOutDuration);
        }

        public void DoCustomZoom(Vector3 offset, float zoomDuration)
        {
            if (cameraFollow)
            {
                DOTween.To(() => cameraFollow.offset, x => cameraFollow.offset = x, offset, zoomDuration)
                    .SetEase(Ease.OutExpo).OnComplete(OnZoomTransitionCompleted);
            }
        }

        private void OnZoomTransitionCompleted()
        {
            OnZoomCompleted?.Invoke();
        }

        public void DoZoomOut()
        {
            DoCustomZoom(zoomOutCameraOffset, zoomOutDuration);
        }

        #endregion
    }
}
