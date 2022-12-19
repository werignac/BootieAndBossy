using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EndWindow : MonoBehaviour
{
	[SerializeField]
	private UnityEvent<long> scoreSet = new UnityEvent<long>();
	[SerializeField]
	private UnityEvent<long> highScoreSet = new UnityEvent<long>();
	[SerializeField]
	private UnityEvent<bool> isHighscoreSet = new UnityEvent<bool>();


    public void Appear(long score, bool is_highscore, long highscore)
	{
		gameObject.SetActive(true);

		scoreSet.Invoke(score);
		highScoreSet.Invoke(highscore);
		isHighscoreSet.Invoke(is_highscore);
	}
}
