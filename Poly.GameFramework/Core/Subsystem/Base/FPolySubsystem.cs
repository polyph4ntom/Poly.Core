using System.Threading.Tasks;

namespace Poly.GameFramework.Core
{
	public abstract class FPolySubsystem
	{
		protected internal virtual Task InitializeSubsystem()
		{
			return Task.CompletedTask;
		}
		
		protected internal virtual void PostInitializeSubsystem() { }

		protected internal virtual void PostWorldPrepared() { }

		protected internal virtual void DisposeSubsystem() { }
	}

	public abstract class FPolyGameInstanceSubsystem : FPolySubsystem
	{
		protected internal FPolyGameInstanceBase GameInstance { get; private set; }

		internal void SetOwner(FPolyGameInstanceBase subsystemOwner)
		{
			GameInstance = subsystemOwner;
		}
		
		public FPolyGameInstanceBase GetGameInstance()
		{
			return GameInstance;
		}

		public T GetGameInstance<T>() where T : FPolyGameInstanceBase
		{
			return GameInstance as T;
		}
	}

	public abstract class FPolyLocalPlayerSubsystem : FPolySubsystem
	{
		protected internal FPolyLocalPlayerBase LocalPlayer { get; private set; }
		
		internal void SetOwner(FPolyLocalPlayerBase subsystemOwner)
		{
			LocalPlayer = subsystemOwner;
		}
		
		public FPolyLocalPlayerBase GetLocalPlayer()
		{
			return LocalPlayer;
		}

		public T GetLocalPlayer<T>() where T : FPolyLocalPlayerBase
		{
			return LocalPlayer as T;
		}
	}
}

namespace Poly.GameFramework.Core
{
	public abstract class FPolyTickableSubsystem : FPolySubsystem
	{
		protected internal abstract void PreTick(float deltaTime);
		protected internal abstract void Tick(float deltaTime);
		protected internal abstract void LateTick(float deltaTime);
		protected internal abstract void FixedTick(float fixedDeltaTime);
	}

	public abstract class FPolyWorldSubsystem : FPolyTickableSubsystem
	{
		protected internal APolyWorldBase World { get; private set; }

		internal void SetOwner(APolyWorldBase subsystemOwner)
		{
			World = subsystemOwner;
		}

		public APolyWorldBase GetWorld()
		{
			return World;
		}

		public T GetWorld<T>() where T : APolyWorldBase
		{
			return World as T;
		}

		protected internal override void PreTick(float deltaTime)
		{
			
		}

		protected internal override void Tick(float deltaTime)
		{
			
		}

		protected internal override void LateTick(float deltaTime)
		{
			
		}

		protected internal override void FixedTick(float fixedDeltaTime)
		{
			
		}
	}
}