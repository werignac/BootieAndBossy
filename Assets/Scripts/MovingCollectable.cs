using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class MovingCollectable : MonoBehaviour
{
	private Rigidbody2D rigid;

	[SerializeField]
	private float moveVelocity = 5f;
	
	[SerializeField]
	public bool isGood = true;

	private const string collectableUpdateKey = "CollectableUpdate";

	private void Start()
	{
		rigid = GetComponent<Rigidbody2D>();

		InitializeMoveDirection();
	}

	private void InitializeMoveDirection()
	{
		rigid.velocity = (Vector2.zero - (Vector2)transform.position).normalized * moveVelocity;
	}

	private void FixedUpdate()
	{
		rigid.velocity = Vector2.ClampMagnitude(rigid.velocity, moveVelocity);

		BroadcastMessage(collectableUpdateKey, rigid.velocity, SendMessageOptions.DontRequireReceiver);
	}
}
