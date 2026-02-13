namespace Poly.GameFramework.Core
{
	public class APolyPlayerControllerBase : APolyMonoBehaviour
	{
		protected FPolyLocalPlayerBase owningPlayer;
		private APolyPawnBase possessedPawn;

		protected internal virtual void InitializePlayerController(FPolyLocalPlayerBase inOwner)
		{
			owningPlayer = inOwner;
		}

		protected internal virtual void Dispose()
		{
			owningPlayer = null;
			UnpossessPawn();
		}

		protected internal virtual void Tick(float deltaTime)
		{
			possessedPawn?.Tick(deltaTime);
		}

		protected internal virtual void FixedTick(float fixedDeltaTime)
		{
			possessedPawn?.FixedTick(fixedDeltaTime);
		}

		protected internal virtual void LateTick(float deltaTime)
		{
			possessedPawn?.LateTick(deltaTime);
		}

		public T GetPawn<T>() where T : APolyPawnBase
		{
			return possessedPawn as T;
		}

		public APolyPawnBase GetPawn()
		{
			return possessedPawn;
		}

		public T GetLocalPlayer<T>() where T : FPolyLocalPlayerBase
		{
			return owningPlayer as T;
		}
		
		public FPolyLocalPlayerBase GetLocalPlayer()
		{
			return owningPlayer;
		}

		internal void PossessPawn(APolyPawnBase newPawn)
		{
			if (possessedPawn != null)
			{
				UnpossessPawn();
			}

			if (newPawn == null)
			{
				return;
			}

			possessedPawn = newPawn;
			newPawn.gameObject.SetActive(true);
			newPawn.OnPossess(this);
		}

		protected void UnpossessPawn()
		{
			possessedPawn?.OnUnpossess(this);
			possessedPawn?.gameObject.SetActive(false);
			possessedPawn = null;
		}
	}
}
