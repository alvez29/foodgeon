using Project.Code.Gameplay.States.StatesLibrary.Player.Grounded.Base;

namespace Project.Code.Gameplay.States.StatesLibrary.Player.Grounded
{
    public class PlayerEatingState : PlayerGroundedState
    {
        #region Override Methods

        protected override void OnPlayerStateEntered(PlayerStateManager manager)
        {
            base.OnPlayerStateEntered(manager);
            
            // Disable movement while eating
            manager.inputHandler.DisableInput();
        }

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

        protected override void OnPlayerStateExited(PlayerStateManager manager)
        {
            // Re-enable movement when eating finished
            manager.inputHandler.EnableInput();
            
            base.OnPlayerStateExited(manager);
        }
        
        #endregion
    }
}
