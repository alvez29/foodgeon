using System;

namespace Project.Code.Gameplay.States.StatesLibrary.Player
{
    public class PlayerDashState : PlayerBaseState
    {

        #region Fields

        private Action _onDashAbilityFinishedDashing;

        #endregion
        
        #region Override Methods

        protected override void OnPlayerStateEntered(PlayerStateManager manager)
        {
            _onDashAbilityFinishedDashing += () => manager.SwitchState(manager.PlayerIdleState);

            manager.dashAbility.OnDashFinished += _onDashAbilityFinishedDashing;
            
            manager.playerStats.SetInvincibility(true);
        }

        protected override void OnPlayerStateExited(PlayerStateManager manager)
        {
            manager.playerStats.SetInvincibility(false);

            manager.dashAbility.OnDashFinished -= _onDashAbilityFinishedDashing;
            
            _onDashAbilityFinishedDashing = null;
        }
        
        #endregion
    }
}
