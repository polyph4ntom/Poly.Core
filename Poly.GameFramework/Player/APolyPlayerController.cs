using Poly.GameFramework.Core;
using Poly.Input;
using UnityEngine;

namespace Poly.GameFramework
{
    [RequireComponent(typeof(APolyInputComponent))]
    public class APolyPlayerController : APolyPlayerControllerBase
    {
        public APolyInputComponent InputComponent { get; private set; }
        
        protected internal override void InitializePlayerController(FPolyLocalPlayerBase inOwner)
        {
            base.InitializePlayerController(inOwner);
            InputComponent = GetComponent<APolyInputComponent>();
            owningPlayer.GetSubsystem<FPolyInputSubsystem>().RegisterInputComponent(InputComponent);
        }
        
        protected internal override void Dispose()
        {
            UnpossessPawn();
            owningPlayer.GetSubsystem<FPolyInputSubsystem>().UnregisterInputComponent(InputComponent);
            InputComponent = null;
            base.Dispose();
        }
    }
}
