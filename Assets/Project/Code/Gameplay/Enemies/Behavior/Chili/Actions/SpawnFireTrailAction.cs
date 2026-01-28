using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;

namespace Project.Code.Gameplay.Enemies.Behavior.Chili.Actions
{
    [Serializable, Unity.Properties.GeneratePropertyBag]
    [NodeDescription(name: "Spawn Fire Trail", story: "Spawn fire trail at current position", category: "Custom/Chili", id: "12345678-1234-5678-9012-345678901234")]
    public class SpawnFireTrailAction : Action
    {
        protected override Status OnStart()
        {
            if (GameObject == null) return Status.Failure;

            var chilliData = GameObject.GetComponent<ChilliBehaviourData>();
            
            if (chilliData == null || chilliData.fireTrailPrefab == null) return Status.Failure;
            
            //TODO: Use pooling
            UnityEngine.Object.Instantiate(chilliData.fireTrailPrefab, GameObject.transform.position, Quaternion.identity);
            return Status.Success;

        }
    }
}
