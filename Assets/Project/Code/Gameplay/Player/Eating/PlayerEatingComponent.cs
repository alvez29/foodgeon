using System;
using System.Collections.Generic;
using NUnit.Framework;
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
            var hitCounts = Physics.OverlapSphereNonAlloc(origin, 10, _hitResults, targetLayer);

            HitboxDebugger.Instance.DrawSphere(origin, 10, Color.yellow, 0.5f);

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

                    PlayerStats.AddEnemyReward(enemyStats.EnemyReward);

                    (PlayerStats as PlayerStats)?.AddToBelly(
                        new EatenEnemyData(enemyStats.EnemyType, enemyStats.Flavor));

                    edibleComponent.OnBeingEaten();

                    _evolutionComponent?.TryEvolving();
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
    }
}