using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Audio;

public class AudioSettingsManager : MonoBehaviour
{
	[SerializeField]
	private AudioMixer mixer;

	private const float lowerVol = -20f;
	private const float upperVol = 2f;
	private const float defaultVolVal = 0.75f;

	private void Start()
	{
		UpdateGroups();
	}

	private void UpdateGroups()
	{
		mixer.SetFloat("masterVol", Mathf.Lerp(lowerVol, upperVol, PlayerPrefs.GetFloat("masterVol", defaultVolVal)));
		mixer.SetFloat("musicVol", Mathf.Lerp(lowerVol, upperVol, PlayerPrefs.GetFloat("musicVol", defaultVolVal)));
		mixer.SetFloat("sfxVol", Mathf.Lerp(lowerVol, upperVol, PlayerPrefs.GetFloat("sfxVol", defaultVolVal)));
	}

	public void SetMaster(float value)
	{
		PlayerPrefs.SetFloat("masterVol", value);
		UpdateGroups();
	}

	public float GetMaster()
	{
		return PlayerPrefs.GetFloat("masterVol", defaultVolVal);
	}

	public void GetMaster(Action<float> callback)
	{
		callback(GetMaster());
	}

	public void SetMusic(float value)
	{
		PlayerPrefs.SetFloat("musicVol", value);
		UpdateGroups();
	}

	// TODO: Simplify with a query interface or make this class broadcast settings.
	public float GetMusic()
	{
		return PlayerPrefs.GetFloat("musicVol", defaultVolVal);
	}

	public void GetMusic(Action<float> callback)
	{
		callback(GetMusic());
	}

	// TODO: Simplify with a query interface or make this class broadcast settings.
	public void SetSFX(float value)
	{
		PlayerPrefs.SetFloat("sfxVol", value);
		UpdateGroups();
	}

	public float GetSFX()
	{
		return PlayerPrefs.GetFloat("sfxVol", defaultVolVal);
	}

	// TODO: Simplify with a query interface or make this class broadcast settings.
	public void GetSFX(Action<float> callback)
	{
		callback(GetSFX());
	}
}
