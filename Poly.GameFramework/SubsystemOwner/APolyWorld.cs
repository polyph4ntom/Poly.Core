using System;
using System.Threading.Tasks;
using Poly.Common;
using Poly.GameFramework.Core;
using Poly.Log;
using Poly.Loop;
using Poly.Save;
using Poly.Settings;
using Poly.UI;

namespace Poly.GameFramework
{
	public sealed class APolyWorld : APolyWorldBase
	{
		protected internal override async Task PrepareWorldLoop(int playerIdx)
		{
			var loop = GetSubsystem<FPolyLoopSubsystem>();
			var modeClassSet = FPolyDevSettingsDatabase.Get<FPolyModeSettings>().RetrieveSet(ScenePath);

			var registrar = (FPolyModelRegistrar) Activator.CreateInstance(modeClassSet.ModelsRegistrar.Type);
			try
			{
				await registrar.RegisterModels(this, playerControllersOnMap[playerIdx].GetLocalPlayer<FPolyLocalPlayerBase>());
			}
			catch (Exception e)
			{
				FPolyLog.Error("Poly.GameFramework", "RegisterModelsFailed: " + e);
				throw;
			}

			FPolySave.LoadGame();
			FPolyApplication.LoadingController.EndLoadingProcess(true);
			loop.ChangeState(modeClassSet.EntryState.Type);
		}
	}
}