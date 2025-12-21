namespace Project.Code.Gameplay.States.StatesLibrary.Player
{
    public class PlayerDashState : PlayerBaseState
    {
        #region Override Methods
        
        protected override void OnPlayerStateUpdate(PlayerStateManager manager)
        {
            //dash to idle
            if (!manager.dashAbility.IsDashing)
            {
                manager.SwitchState(manager.PlayerIdleState);
            }
        }
        
        #endregion
    }
}
