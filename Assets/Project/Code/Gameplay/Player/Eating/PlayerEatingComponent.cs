using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Project.Code.Core;
using Project.Code.Core.Data;
using Project.Code.Core.Interfaces;
using Project.Code.Gameplay.Eating.Base;
using Project.Code.Gameplay.Enemies;
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
        
        [Header("Player eating settings")]
        [SerializeField] protected Vector3 eatingTargetPositionOffest = Vector3.zero;
        [SerializeField] protected float eatingLerpPositionDuration = 0.4f;

        [SerializeField] protected Ease eatingEasingPosition = Ease.Linear;  
        //TODO: Use it to wait time
        [SerializeField] protected float eatingTime = 3f;
        [SerializeField] protected float eatingRadius = 10f;
        
        private PlayerInputHandler _inputHandler;
        private PlayerEvolutionComponent _evolutionComponent;
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

        public override void PerformEatingAction()
        {
            var origin = transform.position + Vector3.forward.normalized * eatingRange;
            var hitCounts = Physics.OverlapSphereNonAlloc(origin, eatingRadius, _hitResults, targetLayer);

            HitboxDebugger.Instance.DrawSphere(origin, eatingRadius, Color.yellow, 0.5f);

            for (var i = 0; i < hitCounts; i++)
            {
                var hitResultObject = _hitResults[i].gameObject;

                if (hitResultObject == gameObject) continue;
                if (TryEating(hitResultObject)) break;
            }
        }

        protected override bool TryEating(GameObject objectToEat)
        {
            if (objectToEat.TryGetComponent(out IEdible edibleComponent))
            {
                //If it is an edible enemy   
                if (objectToEat.TryGetComponent(out EnemyStats enemyStats))
                {
                    if (!enemyStats.CanBeEaten) return false;
                    
                    IsEating = true;

                    transform.DOMove(
                        new Vector3(objectToEat.transform.position.x - eatingTargetPositionOffest.x,
                            objectToEat.transform.position.y - eatingTargetPositionOffest.y,
                            objectToEat.transform.position.z - eatingTargetPositionOffest.z),
                        eatingLerpPositionDuration).SetEase(eatingEasingPosition);
                    
                    PlayerStats.AddEnemyReward(enemyStats.EnemyReward);

                    (PlayerStats as PlayerStats)?.AddToBelly(
                        new EatenEnemyData(enemyStats.EnemyType, enemyStats.Flavor));
                    
                    _evolutionComponent?.TryEvolving();

                    StartCoroutine(
                        Constants.Coroutines.WaitTime(eatingTime, () => OnEatingWaitTimeCompleted(edibleComponent)));
                }
                else
                {
                    edibleComponent.OnBeingEaten();
                }

                return true;
            }

            Debug.Log("Eww... That is not edible");
            return false;
        }

        // ReSharper disable Unity.PerformanceAnalysis
        private void OnEatingWaitTimeCompleted(IEdible edibleComponent)
        {
            IsEating = false;
            edibleComponent?.OnBeingEaten();        
        }

    }
}