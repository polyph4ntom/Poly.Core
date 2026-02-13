using System.Collections.Generic;
using Poly.Settings;
using UnityEngine;
using UnityEngine.Audio;

namespace Poly.Sounds
{
	public enum EPolySoundId
	{
		Hover,
		Click,
		Click2,
		PanelIn,
		PanelOut,
		QuestionPopUp,
		Error
	}

	[System.Serializable]
	public class FAudioClipEntry
	{
		[field:SerializeField]
		public EPolySoundId ID { get; private set; }
		
		[field:SerializeField]
		public AudioClip Clip { get; private set; }
	}

	[System.Serializable]
	public class FPolySoundsRegistry
	{
		[field: SerializeField] 
		public AudioMixer Mixer { get; private set; }
		
		[field: SerializeField] 
		public List<FAudioClipEntry> Sounds { get; private set; }

		public bool TryToRetrieveSound(EPolySoundId id, out AudioClip clip)
		{
			foreach (var sound in Sounds)
			{
				if (id != sound.ID)
				{
					continue;
				}
				
				clip = sound.Clip;
				return true;
			}

			clip = null;
			return false;
		}
	}

	[CreateAssetMenu(menuName = "Polyphantom/Settings/PolySoundsRegistry")]
	public class OPolySoundsRegistryAsset : OPolyDevSettingsAsset<FPolySoundsRegistry> { }
}
