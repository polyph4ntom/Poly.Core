using Poly.Common;
using Poly.Settings;
using UnityEngine;

namespace Poly.GameFramework
{
    [System.Serializable]
    public class FPolyModeSettings
    {
        [field: SerializeField]
        public OPolyModeClassSet GlobalModeSettings { get; private set; }

        [field: SerializeField]
        public FPolySerializableDictionary<FPolySceneReference, OPolyModeClassSet> GlobalSettingsPerScene { get; private set; }
        
        public OPolyModeClassSet RetrieveSet(string scenePath)
        {
            foreach(var sceneSetPair in GlobalSettingsPerScene.Dictionary)
            {
                if (sceneSetPair.Key.ScenePath.Equals(scenePath))
                {
                    return sceneSetPair.Value;
                }
            }

            return GlobalModeSettings;
        }
    }

    [CreateAssetMenu(menuName = "Polyphantom/Settings/Mode Settings")]
    public class OPolyModeSettingsAsset : OPolyDevSettingsAsset<FPolyModeSettings> { }
}
