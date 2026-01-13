using Project.Code.Core;
using UnityEngine;

namespace Project.Code.Gameplay.States.StatesLibrary.Player
{
    public class PlayerHitState : PlayerBaseState
    {
        private float _timer;

        protected override void OnPlayerStateEntered(PlayerStateManager manager)
        {
            Debug.Log($"Entered PlayerHitState");
            _timer = 0f;
            
            // TODO: Trigger Visuals here (Animator string or similar)
            // manager.animator.SetTrigger("Hit");
        }

        protected override void OnPlayerStateUpdate(PlayerStateManager manager)
        {
            _timer += Time.deltaTime;

            if (_timer >= Constants.Hit.PlayerHitStunDuration)
            {
                manager.SwitchState(manager.PlayerIdleState);
            }
        }
    }
}
