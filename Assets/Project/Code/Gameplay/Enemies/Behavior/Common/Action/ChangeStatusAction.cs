using System;
using Unity.Behavior;
using Unity.Properties;
using UnityEngine;

namespace Project.Code.Gameplay.Enemies.Behavior.Common.Action
{
    [Serializable, GeneratePropertyBag]
    [NodeDescription(name: "ChangeStatus", story: "Change [Status] to [NewStatus]", category: "Action", id: "f2c3290e620f22404064d01bd7fa6e5e")]
    public partial class ChangeStatusAction : Unity.Behavior.Action
    {
        [SerializeReference] public BlackboardVariable<EnemyStatus> status;
        [SerializeReference] public BlackboardVariable<EnemyStatus> newStatus;

        protected override Status OnStart()
        {
            status.Value = newStatus;
            
            return Status.Success;
        }
    }
}

