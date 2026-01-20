using System;

namespace Project.Code.Gameplay.States.StatesLibrary.Player.Grounded.Base
{
    public abstract class PlayerGroundedState : PlayerBaseState
    {
        #region Fields

        private Action _onDashAbilityOnDashStarted;
        private Action _onPlayerEatingComponentOnEatingStarted;

        #endregion
        
        #region Override Methods

        protected override void OnPlayerStateEntered(PlayerStateManager manager)
        {
            base.OnPlayerStateEntered(manager);
            
            _onDashAbilityOnDashStarted = () => manager.SwitchState(manager.PlayerDashState);
            _onPlayerEatingComponentOnEatingStarted = () => manager.SwitchState(manager.PlayerEatingState);
            
            manager.DashAbility.OnDashStarted += _onDashAbilityOnDashStarted;
            manager.PlayerEatingComponent.OnEatingStarted += _onPlayerEatingComponentOnEatingStarted;
        }

        protected override void OnPlayerStateExited(PlayerStateManager manager)
        {
            manager.DashAbility.OnDashStarted -= _onDashAbilityOnDashStarted;
            manager.PlayerEatingComponent.OnEatingStarted += _onPlayerEatingComponentOnEatingStarted;
            
            _onDashAbilityOnDashStarted = null;
            _onPlayerEatingComponentOnEatingStarted = null;
            
            base.OnPlayerStateExited(manager);
        }
        
        #endregion
    }
}
