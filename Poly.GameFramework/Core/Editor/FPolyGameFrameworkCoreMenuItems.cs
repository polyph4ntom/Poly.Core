using Poly.GameFramework.Core;
using UnityEditor;

namespace GameFramework.Core.Editor
{
	[InitializeOnLoad]
	public class DisableArchitectureOnPlayMode
	{
		private const string MENU_NAME = "Polyphantom/Disable architecture on Play Mode";

		static DisableArchitectureOnPlayMode()
		{
			EditorApplication.delayCall += () =>
			{
				var isToggled = EditorPrefs.GetBool(FPolyGameFrameworkCoreAssembly.DISABLE_ARCHITECTURE, false);
				Menu.SetChecked(MENU_NAME, isToggled);
			};
		}

		[MenuItem(MENU_NAME, false, 101)]
		private static void ToggleMode()
		{
			var isToggled = !EditorPrefs.GetBool(FPolyGameFrameworkCoreAssembly.DISABLE_ARCHITECTURE, false);
			Menu.SetChecked(MENU_NAME, isToggled);
			EditorPrefs.SetBool(FPolyGameFrameworkCoreAssembly.DISABLE_ARCHITECTURE, isToggled);
		}
	}
}
