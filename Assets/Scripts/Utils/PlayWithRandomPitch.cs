using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayWithRandomPitch : MonoBehaviour
{
	[SerializeField]
	private float lowerBound = 0.8f;
	[SerializeField]
	private float upperBound = 1.2f;

	[SerializeField]
	private bool waitForFinish = true;

	[SerializeField]
	private AudioSource source;

    public void Play()
	{
		source.pitch = Random.Range(lowerBound, upperBound);
		if (!source.isPlaying || !waitForFinish)
			source.Play();
	}
}
