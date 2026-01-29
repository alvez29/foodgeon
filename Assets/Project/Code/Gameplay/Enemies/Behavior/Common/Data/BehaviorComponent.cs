using Project.Code.Core;
using Project.Code.Gameplay.Enemies.Behavior.Common.Workers;
using Project.Code.Gameplay.Stats;
using Unity.Behavior;
using UnityEngine;

namespace Project.Code.Gameplay.Enemies.Behavior.Common.Data
{
    [RequireComponent(typeof(BaseStats))]
    public abstract class BehaviorComponent : MonoBehaviour
    {
        [Header("Patrol and Detection")] 
        public float patrolRadius = 5f;
        public float detectionRadius = 2f;
        public float randomPointsNumber = 3f;
        public float patrolSpeed = 3f;
        public float patrolWaitTime = 2f;
        public LayerMask detectionLayerMask;

        [Header("Subgraphs")] 
        [SerializeField] protected BehaviorGraph attackSubgraph;

        private BehaviorGraphAgent _behaviorGraphAgent;
        protected BaseStats BaseStats;
        
        private void Awake()
        {
            _behaviorGraphAgent = GetComponent<BehaviorGraphAgent>();
            BaseStats = GetComponent<BaseStats>();
            
            var detectorWorker = gameObject.AddComponent<DetectorWorker>();
            detectorWorker.radius = detectionRadius;
            detectorWorker.layerMask = detectionLayerMask;
            
            _behaviorGraphAgent?.SetVariableValue(Constants.Behavior.Blackboard.PatrolSpeedKey, patrolSpeed);
            _behaviorGraphAgent?.SetVariableValue(Constants.Behavior.Blackboard.PatrolWaitTimeKey, patrolWaitTime);
            _behaviorGraphAgent?.SetVariableValue(Constants.Behavior.Blackboard.AttackSubgraphKey, attackSubgraph);
            _behaviorGraphAgent?.SetVariableValue(Constants.Behavior.Blackboard.DetectorWorkerKey, detectorWorker);
            
            if (BaseStats != null) BaseStats.OnDeath += Disconnect;

            OnAwake();
        }

        protected abstract void OnAwake();
        
        private void Disconnect()
        {
            if (_behaviorGraphAgent != null)
            {
                Destroy(_behaviorGraphAgent);
            }
        }
    }
}