using System.Runtime.CompilerServices;
using Poly.Settings;
using UnityEngine;

[assembly: InternalsVisibleTo("Poly.GameFramework")]

namespace Poly.GameFramework.Core
{
	public static class FPolyGameFrameworkCoreAssembly
	{
		public const string DISABLE_ARCHITECTURE = "DisableArchiture";
		public const int LOADING_SCENE_INDEX = 1;
		
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
		public static void LoadAssembly()
		{
			FPolyDevSettingsDatabase.Register<FPolyCoreSettings>("SETTINGS_Core");
		}
	}
}