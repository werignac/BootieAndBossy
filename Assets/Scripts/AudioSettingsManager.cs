using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioSettingsManager : MonoBehaviour
{
	public static AudioSettingsManager instance = null;

	[SerializeField]
	private AudioMixer mixer;

	private const float lowerVol = -20f;
	private const float upperVol = 2f;
	private const float defaultVolVal = 0.75f;


	private void Awake()
	{
		if (instance == this)
			return;
		else if (instance == null)
		{
			instance = this;
			DontDestroyOnLoad(gameObject);
		}
		else
			Destroy(gameObject);
	}

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

	public void SetMusic(float value)
	{
		PlayerPrefs.SetFloat("musicVol", value);
		UpdateGroups();
	}

	public float GetMusic()
	{
		return PlayerPrefs.GetFloat("musicVol", defaultVolVal);
	}

	public void SetSFX(float value)
	{
		PlayerPrefs.SetFloat("sfxVol", value);
		UpdateGroups();
	}

	public float GetSFX()
	{
		return PlayerPrefs.GetFloat("sfxVol", defaultVolVal);
	}
}
