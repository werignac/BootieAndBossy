using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace werignac.Utils
{
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

		public struct TriggerInfo { public Collider2D collider; public Collider2D other; }

		private new Collider2D collider;

		private void Start()
		{
			collider = GetComponent<Collider2D>();
		}

		private void OnTriggerEnter2D(Collider2D collision)
		{
			SendMessageUpwards(triggerEnterMessage, new TriggerInfo { collider = collider, other = collision }, messageOptions);
		}

		private void OnTriggerStay2D(Collider2D collision)
		{
			SendMessageUpwards(triggerStayMessage, new TriggerInfo { collider = collider, other = collision }, messageOptions);
		}

		private void OnTriggerExit2D(Collider2D collision)
		{
			SendMessageUpwards(triggerExitMessage, new TriggerInfo { collider = collider, other = collision }, messageOptions);
		}
	}
}
