using Poly.Common;
using Poly.GameFramework.Core;
using Poly.Log;
using Poly.Settings;
using UnityEngine;
using UnityEngine.UIElements;

namespace Poly.Sounds
{
	public class FPolySoundsSubsystem : FPolyWorldSubsystem
	{
		private const string MUTE_ID = "MUTE";
		public bool IsGameMuted { get; private set; } = false;
		
		protected override void PostInitializeSubsystem()
		{
			if (!PlayerPrefs.HasKey(MUTE_ID))
			{
				return;
			}

			var soundsRegistry = FPolyDevSettingsDatabase.Get<FPolySoundsRegistry>();
			IsGameMuted = PlayerPrefs.GetInt(MUTE_ID) == 1;
			soundsRegistry.Mixer.SetFloat("Volume",  IsGameMuted ? -80f : 0f);
		}

		public void ChangeMuteStatus(bool isMuted)
		{
			var soundsRegistry = FPolyDevSettingsDatabase.Get<FPolySoundsRegistry>();
			soundsRegistry.Mixer.SetFloat("Volume", isMuted ? -80f : 0f);
			PlayerPrefs.SetInt(MUTE_ID, isMuted ? 1 : 0);
			IsGameMuted = isMuted;
		}

		public void PlaySoundOnceById(EPolySoundId id, AudioSource source, bool shouldStopCurrent = true)
		{
			var soundsRegistry = FPolyDevSettingsDatabase.Get<FPolySoundsRegistry>();

			if (!soundsRegistry.TryToRetrieveSound(id, out var sound))
			{
				FPolyLog.Error("Poly.Sounds", $"There is no sound with id {id} in registry");
				return;
			}

			PlaySoundOnce(sound, source, shouldStopCurrent);
		}
		
		public void PlaySoundOnce(AudioClip clip, AudioSource source, bool shouldStopCurrent = true)
		{
			if (source.isPlaying && !shouldStopCurrent)
			{
				return;
			}

			source.Stop();
			source.PlayOneShot(clip);
		}

		public void RegisterSoundToElement(VisualElement element, AudioSource source, EPolySoundId id)
		{
			var soundsRegistry = FPolyDevSettingsDatabase.Get<FPolySoundsRegistry>();

			if (!soundsRegistry.TryToRetrieveSound(id, out var sound))
			{
				FPolyLog.Error("Poly.Sounds", $"There is no sound with id {id} in registry");
				return;
			}
			
			if (id == EPolySoundId.Hover)
			{
				element.RegisterCallback<PointerEnterEvent>(_ =>
				{
					PlaySoundOnce(sound, source);
				});
				return;
			}
			
			element.RegisterCallback<ClickEvent>(_ =>
			{
				PlaySoundOnce(sound, source);
			});
		}
	}
}
