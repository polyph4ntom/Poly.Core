namespace Poly.Loop
{
	public abstract class FPolyLoopState
	{
		protected FPolyLoopSubsystem rootSubsystem;

		public virtual void InitializeState(FPolyLoopSubsystem root)
		{
			rootSubsystem = root;
		}
		
		public virtual void PreTickState(float deltaTime)
		{
			
		}
		
		public virtual void TickState(float deltaTime)
		{
		
		}
		
		public virtual void LateTickState(float deltaTime)
		{
		
		}

		public virtual void FixedTickState(float fixedDeltaTime)
		{
			
		}

		public virtual void DisposeState()
		{
		
		}
	}
}