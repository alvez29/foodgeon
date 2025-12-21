using Project.Code.Gameplay.Player;
using Project.Code.Gameplay.States.StatesLibrary.Player.Grounded.Base;

namespace Project.Code.Gameplay.States.StatesLibrary.Player.Grounded
{
    public class PlayerIdleState : PlayerGroundedState
    {
        #region Override Methods
        
        // ReSharper disable Unity.PerformanceAnalysis
        protected override void OnPlayerStateUpdate(PlayerStateManager manager)
        {
            base.OnPlayerStateUpdate(manager);
            
            // idle to run
            if (manager.movementComponent.IsMoving)
            {
                manager.SwitchState(manager.PlayerRunState);
            }
        }
        
        #endregion
    }
}
