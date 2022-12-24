using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class AudioSettingSlider : MonoBehaviour
{
	private Slider slider;
	
	private enum SoundType { MASTER, MUSIC, SFX}

	[SerializeField]
	private SoundType soundType;

	private void Awake()
	{
		slider = GetComponent<Slider>();
		slider.value = GetAudioValue();
	}

	private float GetAudioValue()
	{
		float value = 1f;

		switch(soundType)
		{
			case SoundType.MASTER:
				value = AudioSettingsManager.instance.GetMaster();
				break;
			case SoundType.MUSIC:
				value = AudioSettingsManager.instance.GetMusic();
				break;
			case SoundType.SFX:
				value = AudioSettingsManager.instance.GetSFX();
				break;
		}

		return value;
	}

	public void ChangeAudioValue(float newValue)
	{
		switch (soundType)
		{
			case SoundType.MASTER:
				AudioSettingsManager.instance.SetMaster(newValue);
				break;
			case SoundType.MUSIC:
				AudioSettingsManager.instance.SetMusic(newValue);
				break;
			case SoundType.SFX:
				AudioSettingsManager.instance.SetSFX(newValue);
				break;
		}
	}


}
