using System;
using DG.Tweening;
using Project.Code.Core;
using Project.Code.Core.Data;
using Project.Code.Core.Interfaces;
using Project.Code.Gameplay.Camera;
using Project.Code.Gameplay.Eating.Base;
using Project.Code.Gameplay.Enemies;
using Project.Code.Gameplay.Player.Camera;
using Project.Code.Gameplay.Player.Stats;
using Project.Code.Utils;
using UnityEngine;

namespace Project.Code.Gameplay.Player.Eating
{
    [RequireComponent(typeof(PlayerStats))]
    [RequireComponent(typeof(PlayerInputHandler))]
    [RequireComponent(typeof(PlayerEvolutionComponent))]
    
    public class PlayerEatingComponent : BaseEatingComponent
    {
        
        public event Action OnEatingStarted;
        public event Action OnEatingFailed;
        public event Action OnEatingCompleted;
        public event Action OnBitePerformed;
        
        [Header("Player eating settings")]
        [SerializeField] protected Vector3 eatingTargetPositionOffest = Vector3.zero;
        [SerializeField] protected float eatingLerpPositionDuration = 0.4f;

        [SerializeField] protected Ease eatingEasingPosition = Ease.Linear;  
        //[SerializeField] protected float eatingTime = 3f;
        [SerializeField] protected float eatingRadius = 10f;
        
        [SerializeField] private PlayerCameraZoom cameraZoom;
        [SerializeField] private PlayerCameraRotator cameraRotator;
        
        private PlayerInputHandler _inputHandler;
        private PlayerEvolutionComponent _evolutionComponent;

        private float _eatingComboTimes;
        private Action _currentComboAction;
        
        private readonly Collider[] _hitResults = new Collider[10];

        #region Unity Functions

        private void Awake()
        {
            PlayerStats = GetComponent<PlayerStats>();
            _inputHandler = GetComponent<PlayerInputHandler>();
            _evolutionComponent = GetComponent<PlayerEvolutionComponent>();
        }

        private void OnEnable()
        {
            if (_inputHandler)
            {
                _inputHandler.OnEatPerformed += PerformEatingAction;
            }
        }

        private void OnDisable()
        {
            if (_inputHandler)
            {
                _inputHandler.OnEatPerformed -= PerformEatingAction;
            }
        }

        #endregion

        #region Public Functions
        
        public override void PerformEatingAction()
        {
            Debug.Log("PerformEatingAction called");
            if (IsEating) return;

            var origin = transform.position + transform.forward.normalized * eatingRange;
            var hitCounts = Physics.OverlapSphereNonAlloc(origin, eatingRadius, _hitResults, targetLayer);

            HitboxDebugger.Instance.DrawSphere(origin, eatingRadius, Color.yellow, 0.5f);

            for (var i = 0; i < hitCounts; i++)
            {
                var hitResultObject = _hitResults[i].gameObject;

                if (hitResultObject == gameObject) continue;
                if (TryEating(hitResultObject)) break;
            }
        }
        
        #endregion

        #region Private Functions

         protected override bool TryEating(GameObject objectToEat)
        {
            if (objectToEat.TryGetComponent(out IEdible edibleComponent))
            {
                OnEatingStarted?.Invoke();

                //If it is an edible enemy   
                if (objectToEat.TryGetComponent(out EnemyStats enemyStats))
                {
                    if (!enemyStats.CanBeEaten) return false;
                    
                    IsEating = true;
                    _inputHandler.DisableInputs(PlayerInputHandler.PlayerConfigurableInputs.Aim,
                        PlayerInputHandler.PlayerConfigurableInputs.Dash, PlayerInputHandler.PlayerConfigurableInputs.Move);
                    MoveToTarget(objectToEat, enemyStats, edibleComponent);
                }
                else
                {
                    edibleComponent.OnBeingEaten();
                }

                return true;
            }

            OnEatingFailed?.Invoke();
            Debug.Log("Eww... That is not edible");
            return false;
        }

        private void MoveToTarget(GameObject objectToEat, EnemyStats enemyStats, IEdible edibleComponent)
        {
            transform.DOMove(
                new Vector3(objectToEat.transform.position.x - eatingTargetPositionOffest.x,
                    objectToEat.transform.position.y - eatingTargetPositionOffest.y,
                    objectToEat.transform.position.z - eatingTargetPositionOffest.z),
                eatingLerpPositionDuration).SetEase(eatingEasingPosition).OnComplete(OnMovedToEatingTarget(enemyStats, edibleComponent));
        }

        private TweenCallback OnMovedToEatingTarget(EnemyStats enemyStats, IEdible edibleComponent)
        {
            return () =>
            {
                _inputHandler.OnEatPerformed -= PerformEatingAction;
                _currentComboAction = () => PerformBite(enemyStats, edibleComponent);
                _inputHandler.OnEatPerformed += _currentComboAction;
            };
        }
        
        private void PerformBite(EnemyStats enemyStats, IEdible edibleComponent)
        {
            _eatingComboTimes++;
            OnBitePerformed?.Invoke();
            DoCameraEffect();

            // If the combo has finished
            if (_eatingComboTimes >= Constants.Stats.Player.EatingComboTimes)
            {
                CompleteEating(enemyStats, edibleComponent);
            }
        }

        private void CompleteEating(EnemyStats enemyStats, IEdible edibleComponent)
        {
            PlayerStats.AddEnemyReward(enemyStats.EnemyReward);
            (PlayerStats as PlayerStats)?.AddToBelly(
                new EatenEnemyData(enemyStats.EnemyType, enemyStats.Flavor));
            _evolutionComponent?.TryEvolving();
            edibleComponent?.OnBeingEaten();
            ResetEatingState();
        }

        private void DoCameraEffect()
        {
            CameraShake.Instance.AddTrauma(0.4f);
            var eatingComboTimes = _eatingComboTimes + 1;
            cameraZoom?.DoCustomZoom(new Vector3(0f, 8f, -10f)/eatingComboTimes, 0.2f);
            cameraRotator?.RotateIncrement(new Vector3(0f, 0f, 1.43f), 0.2f);
        }

        private void ResetEatingState()
        {
            if (_currentComboAction != null)
            {
                _inputHandler.OnEatPerformed -= _currentComboAction;
                _currentComboAction = null;
            }
                
            _inputHandler.OnEatPerformed += PerformEatingAction;
            cameraZoom?.ResetZoom();
            cameraRotator?.ResetRotation();
            _inputHandler.EnableInputs(PlayerInputHandler.PlayerConfigurableInputs.Aim,
                PlayerInputHandler.PlayerConfigurableInputs.Dash, PlayerInputHandler.PlayerConfigurableInputs.Move);
            _eatingComboTimes = 0;
            IsEating = false;
            OnEatingCompleted?.Invoke();
        }

        #endregion
    }
}