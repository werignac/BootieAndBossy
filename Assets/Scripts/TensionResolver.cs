using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(HingeJoint2D))]
public class TensionResolver : MonoBehaviour
{
	[SerializeField, Min(0)]
	private float tensionResolveFactor = 1f;
	[SerializeField]
	private HingeJoint2D _hinge;

    // Update is called once per frame
    void FixedUpdate()
    {
		Rigidbody2D attachedBody = _hinge.attachedRigidbody;
		Rigidbody2D connectedBody = _hinge.connectedBody;

		float hingeAngle = GetAngle();


		float resolutionTorque = -hingeAngle * tensionResolveFactor;

		attachedBody.AddTorque(-resolutionTorque);
		connectedBody.AddTorque(resolutionTorque);

    }

	public float GetAngle()
	{
		Rigidbody2D attachedBody = _hinge.attachedRigidbody;
		Rigidbody2D connectedBody = _hinge.connectedBody;

		// Use the ups of the segments instead of hinge.angle because segments can spawn at weird angles
		// causing them to have an incorrect resting position.

		return Vector2.SignedAngle(attachedBody.transform.up, connectedBody.transform.up);
	}
}
