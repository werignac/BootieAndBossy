using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class TriggerRepeater : MonoBehaviour
{
	[SerializeField]
	private string triggerEnterMessage = "OnChildTriggerEnter";
	[SerializeField]
	private string triggerStayMessage = "OnChildTriggerStay";
	[SerializeField]
	private string triggerExitMessage = "OnChildTriggerExit";

	private const SendMessageOptions messageOptions = SendMessageOptions.DontRequireReceiver;

	private void OnTriggerEnter2D(Collider2D collision)
	{
		SendMessageUpwards(triggerEnterMessage, collision, messageOptions);
	}

	private void OnTriggerStay2D(Collider2D collision)
	{
		SendMessageUpwards(triggerStayMessage, collision, messageOptions);
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		SendMessageUpwards(triggerExitMessage, collision, messageOptions);
	}
}
