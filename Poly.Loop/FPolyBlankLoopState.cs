using Poly.Common;
using Poly.Log;

namespace Poly.Loop
{
	public class FPolyBlankLoopState : FPolyLoopState
	{
		public override void InitializeState(FPolyLoopSubsystem root)
		{
			base.InitializeState(root);
			FPolyLog.Error("Poly.Loop", "PolyBlankState has been loaded. There is no logic there so probably you haven't set proper state at the start on your scene! Check World Settings!");
		}
	}
}