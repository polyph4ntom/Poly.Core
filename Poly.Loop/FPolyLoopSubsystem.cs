using System;
using Poly.Common;
using Poly.GameFramework.Core;
using Poly.Log;

namespace Poly.Loop
{
	public sealed class FPolyLoopSubsystem : FPolyWorldSubsystem
	{
		private FPolyLoopState currentState;
		
		protected override void PreTick(float deltaTime)
		{
			currentState?.PreTickState(deltaTime);
		}

		protected override void Tick(float deltaTime)
		{
			currentState?.TickState(deltaTime);
		}

		protected override void LateTick(float deltaTime)
		{
			currentState?.LateTickState(deltaTime);
		}

		protected override void FixedTick(float fixedDeltaTime)
		{
			currentState?.FixedTickState(fixedDeltaTime);
		}

		protected override void DisposeSubsystem()
		{
			currentState?.DisposeState();
			currentState = null;
			base.DisposeSubsystem();
		}
		
		public void ChangeState<T>() where T : FPolyLoopState
		{
			currentState?.DisposeState();
			currentState = Activator.CreateInstance<T>();
			currentState.InitializeState(this);
		}
		
		
		public void ChangeState(Type type)
		{
			currentState?.DisposeState();
			currentState = (FPolyLoopState) Activator.CreateInstance(type);
		
			if (currentState == null)
			{
				FPolyLog.Error("Poly.Loop", $"Cannot change state to {type}. Type must by of FPolyLoopState");
				return;
			}
		
			currentState.InitializeState(this);
		}
	}
}