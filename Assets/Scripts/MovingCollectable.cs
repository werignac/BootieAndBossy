using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody2D))]
public class MovingCollectable : MonoBehaviour
{
	private Rigidbody2D rigid;

	[SerializeField]
	private float moveVelocity = 5f;
	
	[SerializeField]
	public bool isGood = true;

	private const string collectableUpdateKey = "CollectableUpdate";

	private Coroutine checkMoving = null;
	[SerializeField]
	private float timeStillUntilRandomMove = 5f;

	[SerializeField]
	private bool forTutorial = false;

	private bool IsNotMoving { get { return rigid.velocity.magnitude < 0.5f; } }

	public UnityEvent onDestroy = new UnityEvent();

	private void Start()
	{
		rigid = GetComponent<Rigidbody2D>();

		InitializeMoveDirection();
	}

	private void InitializeMoveDirection()
	{
		if (!forTutorial)
			rigid.velocity = (Vector2.zero - (Vector2)transform.position).normalized * moveVelocity;
	}

	private void FixedUpdate()
	{
		rigid.velocity = Vector2.ClampMagnitude(rigid.velocity, moveVelocity);

		BroadcastMessage(collectableUpdateKey, rigid.velocity, SendMessageOptions.DontRequireReceiver);

		if (!forTutorial)
		{
			if (IsNotMoving && checkMoving == null)
				checkMoving = StartCoroutine(CheckMoving());
		}
	}

	private IEnumerator CheckMoving()
	{
		yield return new WaitForSeconds(timeStillUntilRandomMove);

		if (IsNotMoving)
			rigid.velocity = Quaternion.Euler(0, 0, Random.Range(-30f, 30f)) * -transform.position.normalized * moveVelocity;

		checkMoving = null;
	}

	public void HandleDestroy()
	{
		onDestroy.Invoke();
		Destroy(gameObject);
	}
}
