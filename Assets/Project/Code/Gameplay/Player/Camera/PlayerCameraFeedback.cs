using Project.Code.Core;
using Project.Code.Gameplay.Camera;
using Project.Code.Gameplay.Managers;
using Project.Code.Gameplay.Player.Eating;
using Project.Code.Gameplay.Player.Stats;
using UnityEngine;

namespace Project.Code.Gameplay.Player.Camera
{
    public class PlayerCameraFeedback : MonoBehaviour
    {
        [SerializeField] private PlayerEatingComponent eating;
        [SerializeField] private PlayerStats playerStats;
        private CameraShake _cameraShake;
        private PlayerCameraZoom _cameraZoom;
        private PlayerCameraRotator _cameraRotator;

        private void Awake()
        {
            _cameraShake = this.GetComponentInChildrenOrSelf<CameraShake>();
            _cameraZoom = this.GetComponentInChildrenOrSelf<PlayerCameraZoom>();
            _cameraRotator = this.GetComponentInChildrenOrSelf<PlayerCameraRotator>();
        }


        private void OnEnable()
        {
            eating.OnBitePerformed += OnBitePerformed;
            eating.OnEatingCompleted += OnEatingCompleted;
            GameEvents.OnAnyDamageTaken += OnAnyDamageTaken;
        }

        private void OnDisable()
        {
            eating.OnBitePerformed -= OnBitePerformed;
            eating.OnEatingCompleted -= OnEatingCompleted;
            GameEvents.OnAnyDamageTaken -= OnAnyDamageTaken;
        }

        private void OnBitePerformed()
        {
            _cameraShake?.AddTrauma(0.4f);
            var biteCount = eating.currentBiteNumber + 1;
            _cameraZoom?.DoCustomZoom(new Vector3(0f, 8f, -10f) / biteCount, 0.1f);
            var rotatingIncrement = biteCount == 2 ? 4f : -16f; 
            _cameraRotator?.RotateIncrement(new Vector3(0f, 0f, rotatingIncrement), 0.1f);
        }

        private void OnAnyDamageTaken(float currentHealth, float maxHealth, float damageTaken, GameObject source)
        {
            var trauma = Mathf.Clamp(damageTaken * 0.03f, 0.5f, 0.8f);
            _cameraShake?.AddTrauma(trauma);
        }
            
        private void OnEatingCompleted()
        {
            _cameraZoom?.ResetZoom();
            _cameraRotator?.ResetRotation();
        }
        
    }
}