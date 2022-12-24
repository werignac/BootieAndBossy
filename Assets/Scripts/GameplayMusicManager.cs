using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayMusicManager : MonoBehaviour
{
	[SerializeField]
	private AudioSource source;
	[SerializeField]
	private AudioClip switchClip;
    
	void OnRopeCut()
	{
		source.Stop();
		source.clip = switchClip;
		source.Play();
	}
}
