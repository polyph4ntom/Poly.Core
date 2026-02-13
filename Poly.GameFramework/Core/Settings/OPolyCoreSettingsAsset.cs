using Poly.Common;
using Poly.Settings;
using UnityEngine;

namespace Poly.GameFramework.Core
{
	[System.Serializable]
	public class FPolyCoreSettings
	{
		[field: SerializeField]
		public OPolyCoreClassSet GlobalCoreSettings { get; private set; }

		[field: SerializeField]
		public FPolySerializableDictionary<FPolySceneReference, OPolyCoreClassSet> CoreSettingsPerScene { get; private set; }

		public OPolyCoreClassSet RetrieveSet(string scenePath)
		{
			foreach(var sceneSetPair in CoreSettingsPerScene.Dictionary)
			{
				if (sceneSetPair.Key.ScenePath.Equals(scenePath))
				{
					return sceneSetPair.Value;
				}
			}

			return GlobalCoreSettings;
		}
	}

	[CreateAssetMenu(menuName = "Polyphantom/Settings/Core Settings")]
	public class OPolyCoreSettingsAsset : OPolyDevSettingsAsset<FPolyCoreSettings> { }
}
