using System;
using System.Collections.Generic;
using Project.Code.Gameplay.Enemies.Behavior.Common.Data;
using Unity.Behavior;
using Unity.Properties;
using UnityEngine;

namespace Project.Code.Gameplay.Enemies.Behavior.Common.Action
{
    [Serializable, GeneratePropertyBag]
    [NodeDescription(name: "FindPatrolPoints", story: "Calculate [Number] random patrol points and assign to [PatrolPoints]", category: "Action", id: "a9170dc2601f425c3dfac001fdb68754")]
    public partial class FindPatrolPointsAction : Unity.Behavior.Action
    {
        [SerializeReference] public BlackboardVariable<List<GameObject>> patrolPoints;
        
        protected override Status OnStart()
        {
            var behaviourData = GameObject.GetComponent<BehaviorComponent>();
            
            if (behaviourData == null)
            {
                Debug.LogError("BehaviourData component not found on AI GameObject.");
                return Status.Failure;
            }

            if (patrolPoints.Value == null)
                patrolPoints.Value = new List<GameObject>();
            else
                patrolPoints.Value.Clear();

            var origin = GameObject.transform.position;
            var radius = behaviourData.patrolRadius;

            for (var i = 0; i < behaviourData.randomPointsNumber; i++)
            {
                var randomCircle = UnityEngine.Random.insideUnitCircle * radius;
                var pointPosition = origin + new Vector3(randomCircle.x, 0f, randomCircle.y);

                var patrolPoint = new GameObject($"PatrolPoint_{i}")
                {
                    transform =
                    {
                        position = pointPosition
                    }
                };

                patrolPoints.Value.Add(patrolPoint);
            }

            return Status.Success;
        }

        protected override Status OnUpdate()
        {
            return Status.Success;
        }

        protected override void OnEnd()
        {
        }
    }
}

