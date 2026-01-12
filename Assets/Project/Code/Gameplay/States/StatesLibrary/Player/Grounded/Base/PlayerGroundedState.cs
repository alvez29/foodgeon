using Project.Code.Gameplay.Player;

namespace Project.Code.Gameplay.States.StatesLibrary.Player.Grounded.Base
{
    public abstract class PlayerGroundedState : PlayerBaseState
    {
        #region Override Methods
        
        protected override void OnPlayerStateUpdate(PlayerStateManager manager)
        {
            // any grounded to stash
            if (manager.dashAbility.IsDashing)
            {
                manager.SwitchState(manager.PlayerDashState);
            }
            else if (manager.playerEatingComponent.IsEating)
            {
                manager.SwitchState(manager.PlayerEatingState);
            }
        }
        
        #endregion
    }
}
