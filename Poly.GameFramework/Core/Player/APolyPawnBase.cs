namespace Poly.GameFramework.Core
{
	public class APolyPawnBase : APolyMonoBehaviour
	{
		protected APolyPlayerControllerBase owningPlayerController;
		
		protected internal virtual void OnPossess(APolyPlayerControllerBase owner)
		{
			owningPlayerController = owner;
		}
		
		protected internal virtual void Tick(float deltaTime)
		{
			
		}

		protected internal virtual void FixedTick(float fixedDeltaTime)
		{
			
		}

		protected internal virtual void LateTick(float deltaTime)
		{
			
		}
		
		protected internal virtual void OnUnpossess(APolyPlayerControllerBase owner)
		{
			owningPlayerController = null;
		}
	}
}