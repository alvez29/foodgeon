using System;
using DG.Tweening;
using UnityEngine;

namespace Project.Code.Gameplay.Player.Camera
{
    public class PlayerCameraRotator : MonoBehaviour
    {
        public event Action OnRotationEnded;
        
        private Vector3 _initialRotation;

        #region Unity Functions

        private void Awake()
        {
            _initialRotation = transform.eulerAngles;
        }

        #endregion
        
        #region Public Functions

        public void ResetRotation(float duration = 0.2f)
        {
            CustomRotateCamera(_initialRotation, duration);
        }


        public void CustomRotateCamera(Vector3 rotation, float duration)
        {
            transform.DORotate(rotation, duration).OnComplete(() => OnRotationEnded?.Invoke());
        }

        public void RotateIncrement(Vector3 rotationIncrement, float duration)
        {
            var actualRotation = transform.eulerAngles;
            CustomRotateCamera(actualRotation + rotationIncrement, duration);
        }
        
        #endregion
        
    }
}
