using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ScoreTracker : MonoBehaviour
{

	[SerializeField]
	private UnityEvent<long> onScoreChange = new UnityEvent<long>();
	private long _score;

	private bool counting = true;

	private const long collectableScore = 100;
	private const long timeScore = 1;

	// Between-Frame Data
	private float time = 0f;

    private long Score
	{
		get { return _score; }
		set
		{
			_score = value;
			onScoreChange.Invoke(value);
		}
	}

	private void Update()
	{
		if (!counting)
			return;

		time += Time.deltaTime;

		if (time > 1)
		{
			int scoreAddition = (int)time;
			Score += scoreAddition * timeScore;
			time = time - scoreAddition;
		}
	}

	private void OnRopeCut()
	{
		counting = false;
	}

	private void OnCollectableCollected()
	{
		if (counting)
			Score += collectableScore;
	}
}
