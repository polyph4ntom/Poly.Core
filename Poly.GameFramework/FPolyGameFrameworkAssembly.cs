using Poly.Settings;
using UnityEngine;

namespace Poly.GameFramework
{
	public static class FPolyGameFrameworkAssembly
	{
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
		public static void LoadAssembly()
		{
			FPolyDevSettingsDatabase.Register<FPolyModeSettings>("SETTINGS_Mode");
		}
	}
}