using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(TextMeshProUGUI))]
public class MainMenuHighScoreDisplay : MonoBehaviour
{
	TextMeshProUGUI text;
	private void Start()
	{
		text = GetComponent<TextMeshProUGUI>();
		QueryHighScore();
	}

	public void QueryHighScore()
	{
		if (PlayerPrefs.HasKey(ScoreTracker.highscoreKey))
			text.text = "High Score: " + PlayerPrefs.GetInt(ScoreTracker.highscoreKey) + "\"";
		else
			text.text = "";
	}
}
