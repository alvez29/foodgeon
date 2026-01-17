using Project.Code.Gameplay.Player;
using Project.Code.Gameplay.States.StatesLibrary.Player.Grounded.Base;

namespace Project.Code.Gameplay.States.StatesLibrary.Player.Grounded
{
    public class PlayerEatingState : PlayerGroundedState
    {
        #region Override Methods

        // ReSharper disable Unity.PerformanceAnalysis
        protected override void OnPlayerStateUpdate(PlayerStateManager manager)
        {
            base.OnPlayerStateUpdate(manager);
            
            // idle to run
            if (!manager.playerEatingComponent.IsEating)
            {
                manager.SwitchState(manager.PlayerIdleState);
            }
        }
        
        #endregion
    }
}
