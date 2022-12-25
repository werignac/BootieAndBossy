using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ControlsGate : MonoBehaviour
{
	public static readonly string gateName = "CheckControls";

	[SerializeField]
	private UnityEvent gatePass = new UnityEvent();
	[SerializeField]
	private UnityEvent gateFail = new UnityEvent();

	public void CheckGate()
	{
		if (PlayerPrefs.GetInt(gateName, 0) > 0)
			gatePass.Invoke();
		else
		{
			gateFail.Invoke();
			PlayerPrefs.SetInt(gateName, 1);
		}
	}
}
