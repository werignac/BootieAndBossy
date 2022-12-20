using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace werignac.Utils
{
	[RequireComponent(typeof(Collider2D))]
	public class CollisionRepeater : MonoBehaviour
	{
		[SerializeField]
		private string triggerEnterMessage = "OnChildCollisionEnter";
		[SerializeField]
		private UnityEvent<Collision2D> onCollisionEnter = new UnityEvent<Collision2D>();
		[SerializeField]
		private string triggerStayMessage = "OnChildCollisionStay";
		[SerializeField]
		private UnityEvent<Collision2D> onCollisionStay = new UnityEvent<Collision2D>();
		[SerializeField]
		private string triggerExitMessage = "OnChildCollisionExit";
		[SerializeField]
		private UnityEvent<Collision2D> onCollisionExit = new UnityEvent<Collision2D>();

		private const SendMessageOptions messageOptions = SendMessageOptions.DontRequireReceiver;

		private void OnCollisionEnter2D(Collision2D collision)
		{
			onCollisionEnter.Invoke(collision);
			SendMessageUpwards(triggerEnterMessage, collision, messageOptions);
		}

		private void OnCollisionStay2D(Collision2D collision)
		{
			onCollisionStay.Invoke(collision);
			SendMessageUpwards(triggerStayMessage, collision, messageOptions);
		}

		private void OnCollisionExit2D(Collision2D collision)
		{
			onCollisionExit.Invoke(collision);
			SendMessageUpwards(triggerExitMessage, collision, messageOptions);
		}
	}
}
