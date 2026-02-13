using UnityEngine;

namespace Poly.GameFramework.Core
{
	public abstract class APolyLoadingComponent : MonoBehaviour
	{
		public abstract void HideWorld(bool immediate = false);
		public abstract void ShowWorld(bool immediate = false);
		public abstract void SetLevelData(string levelName, string levelDescription);
		public abstract void FillProgressBar();
	}
}