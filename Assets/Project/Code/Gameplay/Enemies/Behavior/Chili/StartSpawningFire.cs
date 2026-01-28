using System;
using Unity.Behavior;
using Unity.Properties;
using UnityEngine;

#if UNITY_EDITOR
namespace Project.Code.Gameplay.Enemies.Behavior.Chili
{
    [CreateAssetMenu(menuName = "Behavior/Event Channels/StartSpawnigFire")]
#endif
    [Serializable, GeneratePropertyBag]
    [EventChannelDescription(name: "StartSpawnigFire", message: "Start spawning fire", category: "Events", id: "61c7aaaf9f6e19ede573296e57c01a46")]
    public sealed partial class StartSpawningFire : EventChannel { }
}

