using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighScoreResetter : MonoBehaviour
{
    public void ResetHighScore()
	{
		PlayerPrefs.DeleteKey(ScoreTracker.highscoreKey);
	}
}
