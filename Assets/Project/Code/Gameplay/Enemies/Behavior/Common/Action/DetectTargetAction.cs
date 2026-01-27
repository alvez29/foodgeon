using System;
using Project.Code.Core;
using Project.Code.Gameplay.Enemies.Behavior.Common.Workers;
using Unity.Behavior;
using Unity.Properties;
using UnityEngine;

namespace Project.Code.Gameplay.Enemies.Behavior.Common.Action
{
    [Serializable, GeneratePropertyBag]
    [NodeDescription(name: "DetectTarget", story: "Check if [DetectorWorker] has found [Target] and inject it to [AttackSubgraph]", category: "Action", id: "fd44502b04b718b706140ff76d6e7548")]
    public partial class DetectTargetAction : Unity.Behavior.Action
    {
        [SerializeReference] public BlackboardVariable<DetectorWorker> detectorWorker;
        [SerializeReference] public BlackboardVariable<GameObject> target;
        [SerializeReference] public BlackboardVariable<BehaviorGraph> attackSubgraph;

        [SerializeReference] public bool continuousDetection;

        protected override Status OnStart()
        {
            return CheckTarget();
        }

        private Status CheckTarget()
        {
            var detectedTarget = detectorWorker.Value.TryDetectingObject();

            if (detectedTarget != null)
            {
                target.Value = detectedTarget;
                attackSubgraph.Value.BlackboardReference.SetVariableValue(Constants.Behavior.Blackboard.TargetKey, detectedTarget);
                return Status.Success;
            }
            
            return continuousDetection ? Status.Running : Status.Failure;
        }
    }
}

