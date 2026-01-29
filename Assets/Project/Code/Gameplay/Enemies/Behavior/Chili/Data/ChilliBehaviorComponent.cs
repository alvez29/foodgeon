using Project.Code.Core;
using Project.Code.Gameplay.Enemies.Behavior.Common.Data;
using UnityEngine;

namespace Project.Code.Gameplay.Enemies.Behavior.Chili.Data
{
    public class ChilliBehaviorComponent : BehaviorComponent
    {
        [Header("References")] public GameObject fireTrailPrefab;

        [Header("Settings")] public float fleeDistance = 20f;
        public float fleeRadius = 10f;
        public float afterFleeWaitTime = 2f;
        public float fleeDistanceToTarget = 3f;
        public float fireTrailRate = 1f;

        protected override void OnAwake()
        {
            attackSubgraph?.BlackboardReference.SetVariableValue(Constants.Behavior.Blackboard.AfterFleeWaitTimeKey,
                afterFleeWaitTime);
            attackSubgraph?.BlackboardReference.SetVariableValue(Constants.Behavior.Blackboard.FleeSpeedKey,
                BaseStats.Speed);
            attackSubgraph?.BlackboardReference.SetVariableValue(Constants.Behavior.Blackboard.FleeDistanceToTargetKey,
                fleeDistanceToTarget);
            attackSubgraph?.BlackboardReference.SetVariableValue(Constants.Behavior.Blackboard.FireTrailRateKey,
                fireTrailRate);
        }
    }
}