using Poly.Settings;
using UnityEngine;

namespace Poly.Sounds
{
	public static class FPolySoundsAssembly
	{
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
		public static void LoadAssembly()
		{
			FPolyDevSettingsDatabase.Register<FPolySoundsRegistry>("SETTINGS_Sounds");
		}
	}
}
