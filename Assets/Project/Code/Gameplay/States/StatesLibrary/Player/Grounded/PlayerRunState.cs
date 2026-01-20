using Project.Code.Gameplay.States.StatesLibrary.Player.Grounded.Base;

namespace Project.Code.Gameplay.States.StatesLibrary.Player.Grounded
{
    public class PlayerRunState : PlayerGroundedState
    {
        protected override void OnPlayerStateUpdate(PlayerStateManager manager)
        {
            base.OnPlayerStateUpdate(manager);
            
            if (!manager.MovementComponent.IsMoving)
            {
                manager.SwitchState(manager.PlayerIdleState);
            }
        }
    }
}
