using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPrefSetter : MonoBehaviour
{
	[SerializeField]
	private string prefName;


	public void PrefSetInt(int v)
	{
		PlayerPrefs.SetInt(prefName, v);
	}
}
