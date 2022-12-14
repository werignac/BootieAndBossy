using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ScoreTracker : MonoBehaviour
{
	public static readonly string highscoreKey = "HighScore";

	[SerializeField]
	private UnityEvent<long> onScoreChange = new UnityEvent<long>();
	[SerializeField]
	private UnityEvent<long, bool, long> onScoreFinish = new UnityEvent<long, bool, long>();
	private long _score = 12;

	private bool counting = true;

	private const long collectableScore = 2;
	private const bool countTime = false;
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

	private void Start()
	{
		onScoreChange.Invoke(Score);
	}

	private void Update()
	{
		if (!counting || !countTime)
			return;

		time += Time.deltaTime;

		if (time > 5)
		{
			int scoreAddition = ((int)time) % 5;
			Score += scoreAddition * timeScore;
			time = time - scoreAddition * 5;
		}
	}

	private void OnRopeCut()
	{
		counting = false;
		long previousScore = (long) PlayerPrefs.GetInt(highscoreKey, 0);

		bool new_highscore = Score > previousScore;

		if (new_highscore)
		{
			PlayerPrefs.SetInt(highscoreKey, (int)Score);
			previousScore = Score;
		}

		onScoreFinish.Invoke(Score, new_highscore, previousScore);
	}

	private void OnCollectableCollected()
	{
		if (counting)
			Score += collectableScore;
	}
}
