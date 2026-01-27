using System;
using Project.Code.Gameplay.Enemies.Behavior.Common;
using Unity.Behavior;
using UnityEngine;
using UnityEngine.AI;
using Action = Unity.Behavior.Action;

namespace Project.Code.Gameplay.Enemies.Behavior.Chili.Actions
{
    [Serializable, Unity.Properties.GeneratePropertyBag]
    [NodeDescription(name: "Get Flee Position", story: "Find flee position from [Target] to [FleePosition]", category: "Custom/Chili", id: "98765432-1234-5678-9012-345678901234")]
    public class GetFleePositionAction : Action
    {
        [SerializeReference] public BlackboardVariable<GameObject> target;
        [SerializeReference] public BlackboardVariable<Vector3> fleePosition;

        protected override Status OnStart()
        {
            if (GameObject == null) return Status.Failure;
            
            var targetObj = target.Value;

            if (targetObj == null)
            {
                return Status.Failure;
            }

            var chilliData = GameObject.GetComponent<ChilliBehaviourData>();
            
            if (chilliData == null) 
            {
                Debug.LogWarning("ChiliReferences component missing on Agent.");
                return Status.Failure;
            }

            var directionToTarget = (GameObject.transform.position - targetObj.transform.position).normalized;
            
            if (directionToTarget == Vector3.zero) directionToTarget = UnityEngine.Random.onUnitSphere;
            
            directionToTarget.y = 0;

            var potentialPos = GameObject.transform.position + directionToTarget * chilliData.fleeDistance;

            if (NavMesh.SamplePosition(potentialPos, out NavMeshHit hit, chilliData.fleeRadius, NavMesh.AllAreas))
            {
                fleePosition.Value = hit.position;
                return Status.Success;
            }

            return Status.Failure;
        }
    }
}
