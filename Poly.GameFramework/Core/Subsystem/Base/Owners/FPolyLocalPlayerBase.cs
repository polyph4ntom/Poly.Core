namespace Poly.GameFramework.Core
{
    public class FPolyLocalPlayerBase : FPolySubsystemOwner<FPolyLocalPlayerSubsystem>
    {
        public APolyPlayerControllerBase CurrentPlayerController { get; protected set; }
        
        protected internal override void PostSubsystemInitialize(FPolyLocalPlayerSubsystem subsystem)
        {
            subsystem.SetOwner(this);
        }

        internal void PreparePlayerController(APolyWorldBase inWorld, APolyPlayerControllerBase playerControllerType)
        {
            if (CurrentPlayerController != null)
            {
                CurrentPlayerController.Dispose();
                inWorld.DespawnObject(CurrentPlayerController);
            }
            
            var newPlayerController = inWorld.SpawnPlayerController(playerControllerType);
            newPlayerController.InitializePlayerController(this);
			         
            CurrentPlayerController = newPlayerController;
            inWorld.SwitchReplacer(0,0);
        }
    }
}
