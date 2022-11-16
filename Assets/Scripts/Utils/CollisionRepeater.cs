using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace werignac.Utils
{
	[RequireComponent(typeof(Collider2D))]
	public class CollisionRepeater : MonoBehaviour
	{
		[SerializeField]
		private string triggerEnterMessage = "OnChildCollisionEnter";
		[SerializeField]
		private string triggerStayMessage = "OnChildCollisionStay";
		[SerializeField]
		private string triggerExitMessage = "OnChildCollisionExit";

		private const SendMessageOptions messageOptions = SendMessageOptions.DontRequireReceiver;

		private void OnCollisionEnter2D(Collision2D collision)
		{
			SendMessageUpwards(triggerEnterMessage, collision, messageOptions);
		}

		private void OnCollisionStay2D(Collision2D collision)
		{
			SendMessageUpwards(triggerStayMessage, collision, messageOptions);
		}

		private void OnCollisionExit2D(Collision2D collision)
		{
			SendMessageUpwards(triggerExitMessage, collision, messageOptions);
		}
	}
}
