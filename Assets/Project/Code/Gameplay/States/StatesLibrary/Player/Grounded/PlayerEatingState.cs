using System;
using Project.Code.Gameplay.States.StatesLibrary.Player.Grounded.Base;

namespace Project.Code.Gameplay.States.StatesLibrary.Player.Grounded
{
    public class PlayerEatingState : PlayerGroundedState
    {
        #region Fields

        private Action _onPlayerEatingComponentFinishedEating;

        #endregion
        
        
        #region Override Methods

        protected override void OnPlayerStateEntered(PlayerStateManager manager)
        {
            base.OnPlayerStateUpdate(manager);
            
            _onPlayerEatingComponentFinishedEating = () => manager.SwitchState(manager.PlayerIdleState);
            
            manager.playerEatingComponent.OnEatingCompleted += _onPlayerEatingComponentFinishedEating;
        }

        protected override void OnPlayerStateExited(PlayerStateManager manager)
        {
            manager.playerEatingComponent.OnEatingCompleted -= _onPlayerEatingComponentFinishedEating;

            _onPlayerEatingComponentFinishedEating = null;
            
            base.OnPlayerStateExited(manager);
        }
        
        #endregion
    }
}
