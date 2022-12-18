using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(Rigidbody2D))]
public class RopeSegment : MonoBehaviour
{
	[SerializeField]
	public HingeJoint2D connectorA;
	[SerializeField]
	public HingeJoint2D connectorB;

	[Min(0.001f)]
	private float targetLength;

	private bool lockedLength = false;

	private static float lengthChangeRate = 1f;

	private CapsuleCollider2D capsuleCollider;
	new private Rigidbody2D rigidbody;

	private Vector3 Axis { get {
			Vector3 difference = RelPos(connectorA) - RelPos(connectorB);
			if (difference.magnitude > 0)
				return Vector3.Normalize(difference);
			else
				return transform.up;
		} }
	private float Length { get { return Math.Max(Vector3.Distance(RelPos(connectorA), RelPos(connectorB)), 0.01f); } }

	private bool IsIncoming(HingeJoint2D connector)
	{
		return connector.connectedBody == rigidbody;
	}

	private Vector3 RelPos(HingeJoint2D connector)
	{
		if (IsIncoming(connector))
			return connector.connectedAnchor;
		else
			return connector.anchor;
	}

	private void SetAnchor(HingeJoint2D connector, Vector3 pos)
	{
		if (IsIncoming(connector))
			connector.connectedAnchor = pos;
		else
			connector.anchor = pos;
	}

	private void SetTargetLength(float _targetLength)
	{
		targetLength = _targetLength;
		lockedLength = false;
	}

	public void Initialze(RopeSegment prevSeg, RopeSegment nextSeg)
	{
		HingeJoint2D prevJoint = prevSeg.connectorA;
		Rigidbody2D nextRigid = nextSeg.GetComponent<Rigidbody2D>();

		connectorA = GetComponent<HingeJoint2D>();
		connectorB = prevJoint;

		Vector2 inPos = prevSeg.transform.TransformPoint(prevJoint.anchor);

		transform.position = inPos;

		connectorB.connectedAnchor = transform.InverseTransformPoint(inPos);
		connectorB.connectedBody = GetComponent<Rigidbody2D>();

		connectorA.connectedBody = nextRigid;
		connectorA.anchor = transform.InverseTransformPoint(inPos);
		connectorA.connectedAnchor = nextRigid.transform.InverseTransformPoint(inPos);
		nextSeg.connectorB = connectorA;
	}

	public void Merge()
	{
		RopeSegment nextSeg = connectorA.connectedBody.GetComponent<RopeSegment>();
		RopeSegment prevSeg = connectorB.attachedRigidbody.GetComponent<RopeSegment>();

		// Find Middle
		Vector3 middlePos = transform.position;

		// Set nextSeg and prevSeg to point to the middle
		Vector3 prevSegBackPos = prevSeg.transform.TransformPoint(prevSeg.connectorB.connectedAnchor);
		Vector3 nextSegFrontPos = nextSeg.transform.TransformPoint(nextSeg.connectorA.anchor);

		Vector3 prevSegPosition = (middlePos + prevSegBackPos) / 2f;
		Vector3 prevSegDiff = (middlePos - prevSegBackPos).normalized;
		Vector3 nextSegPosition = (middlePos + nextSegFrontPos) / 2f;
		Vector3 nextSegDiff = (nextSegFrontPos - middlePos).normalized;

		float prevSegAngle = Mathf.Atan2(prevSegDiff.y, prevSegDiff.x) * Mathf.Rad2Deg;
		float nextSegAngle = Mathf.Atan2(nextSegDiff.y, nextSegDiff.x) * Mathf.Rad2Deg;

		prevSeg.transform.position = prevSegPosition;
		prevSeg.transform.rotation = Quaternion.Euler(0, 0, prevSegAngle - 90f);
		prevSeg.connectorB.connectedAnchor = prevSeg.transform.InverseTransformPoint(prevSegBackPos);

		nextSeg.transform.position = nextSegPosition;
		nextSeg.transform.rotation = Quaternion.Euler(0, 0, nextSegAngle - 90f);
		nextSeg.connectorA.anchor = nextSeg.transform.InverseTransformPoint(nextSegFrontPos);

		// Make prevSeg's connectorB nextSeg's connector A
		prevSeg.connectorA.anchor = prevSeg.transform.InverseTransformPoint(middlePos);
		prevSeg.connectorA.connectedAnchor = nextSeg.transform.InverseTransformPoint(middlePos);
		prevSeg.connectorA.connectedBody = nextSeg.GetComponent<Rigidbody2D>();
		nextSeg.connectorB = prevSeg.connectorA;

		// Destroy self
		DestroyImmediate(gameObject);
	}

	private void Start()
	{
		capsuleCollider = GetComponentInChildren<CapsuleCollider2D>();
		rigidbody = GetComponent<Rigidbody2D>();
	}

	// Update is called once per frame
	void Update()
    {
		if (!lockedLength)
		{
			float length = Length;
			float lengthDiff = targetLength - length;
			if (Mathf.Abs(lengthDiff) > 0.01f)
			{
				float maxDiff = Time.deltaTime * lengthChangeRate;

				float deltaLength = Mathf.Clamp(lengthDiff, -maxDiff, maxDiff);

				Vector3 deltaAnchor = Axis * deltaLength;

				SetAnchor(connectorA, RelPos(connectorA) + deltaAnchor / 2);
				SetAnchor(connectorB, RelPos(connectorB) - deltaAnchor / 2);
				capsuleCollider.size = new Vector2(capsuleCollider.size.x, length + deltaLength + capsuleCollider.size.x);
			}
			else
				lockedLength = true;
		}
	}
}
