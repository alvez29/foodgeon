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
            
            manager.dashAbility.OnDashStarted += _onDashAbilityOnDashStarted;
            manager.playerEatingComponent.OnEatingStarted += _onPlayerEatingComponentOnEatingStarted;
        }

        protected override void OnPlayerStateExited(PlayerStateManager manager)
        {
            manager.dashAbility.OnDashStarted -= _onDashAbilityOnDashStarted;
            manager.playerEatingComponent.OnEatingStarted += _onPlayerEatingComponentOnEatingStarted;
            
            _onDashAbilityOnDashStarted = null;
            _onPlayerEatingComponentOnEatingStarted = null;
            
            base.OnPlayerStateExited(manager);
        }
        
        #endregion
    }
}
