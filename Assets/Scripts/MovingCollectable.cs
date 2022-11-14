using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class MovingCollectable : MonoBehaviour
{
	private Rigidbody2D rigid;

	[SerializeField]
	private float moveVelocity = 5f;

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
		transform.rotation = Quaternion.LookRotation(Vector3.forward, Quaternion.Euler(0, 0, 90f) * rigid.velocity); 
	}
}
