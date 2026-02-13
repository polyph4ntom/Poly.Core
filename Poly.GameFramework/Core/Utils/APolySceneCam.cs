using UnityEngine;

namespace Poly.GameFramework.Core
{
	[DisallowMultipleComponent]
	public sealed class APolySceneCam : APolyMonoBehaviour
	{
		private void Start()
		{
			Destroy(gameObject);
		}
	}
}