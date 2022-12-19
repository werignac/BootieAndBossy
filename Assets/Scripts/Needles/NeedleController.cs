using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class NeedleController : MonoBehaviour
{
	[Header("Movement Rates")]
	[SerializeField]
	private float thrustRate = 5f;
	[SerializeField]
	private float maxVelocity = 10f;
	[SerializeField]
	private float turnRate = 10f;
	[SerializeField]
	private float maxAngularVelocity = 30f;

	private bool takeInput = true;

	// Between-Frame Data
	private float storedThrust = 0;
	private float storedTorque = 0;

	//Component References
	private Rigidbody2D rigid;

	private void Start()
	{
		rigid = GetComponent<Rigidbody2D>();
	}

	private void ReceiveInput(NeedleInputData data)
	{
		storedThrust = data.thrust;
		storedTorque = data.torque;
	}

	private void FixedUpdate()
	{
		if (!takeInput)
			return;

		Vector2 thrustDirection = transform.up;
		float torqueDirection = -1f;

		Vector2 goalVelocity = thrustDirection * storedThrust * maxVelocity;
		float goalAngularVelocity = torqueDirection * storedTorque * maxAngularVelocity;

		Vector2 currentVelocity = rigid.velocity;
		float currentAngularVelocity = rigid.angularVelocity;

		Vector2 velocityDiff = goalVelocity - currentVelocity;
		float angularVelocityDiff = goalAngularVelocity - currentAngularVelocity;

		rigid.AddForce(Vector2.ClampMagnitude(velocityDiff, thrustRate * Time.fixedDeltaTime) * rigid.mass, ForceMode2D.Force);
		float angularVelocityLimit = turnRate * Time.fixedDeltaTime;
		rigid.AddTorque(Mathf.Clamp(angularVelocityDiff, -angularVelocityLimit, angularVelocityLimit) * rigid.mass, ForceMode2D.Force);
	}

	private void OnRopeCut()
	{
		takeInput = false;
	}
}
