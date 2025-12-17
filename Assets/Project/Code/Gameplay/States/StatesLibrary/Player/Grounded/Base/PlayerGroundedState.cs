using Project.Code.Gameplay.Player;

namespace Project.Code.Gameplay.States.StatesLibrary.Player.Grounded.Base
{
    public abstract class PlayerGroundedState : PlayerBaseState
    {
        protected override void OnPlayerStateUpdate(PlayerStateManager manager)
        {
            // any grounded to stash
            if (manager.dashAbility.IsDashing)
            {
                manager.SwitchState(manager.PlayerDashState);
            }
        }
    }
}
