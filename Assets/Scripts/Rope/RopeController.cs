using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using werignac.Utils;

namespace werignac.Rope
{
	struct RopeInfo { public HingeJoint2D[] joints; public GameObject needle_first; public GameObject needle_last; }

	public class RopeController : MonoBehaviour
	{
		private HingeJoint2D[] joints;

		[SerializeField]
		private GameObject segmentPrefab;

		[SerializeField]
		private GameObject needle_first;
		[SerializeField]
		private GameObject needle_last;

		[SerializeField, Range(0.5f, 10)]
		private float ropeLength = 4f;

		[SerializeField, Range(1, 10)]
		private float segmentsPerLength = 1;

		private void Start()
		{
			joints = GetComponentsInChildren<HingeJoint2D>();
		}

		private void Update()
		{
			CheckRopeSegments();
			BroadcastMessage("SetTargetLength", 1 / segmentsPerLength, SendMessageOptions.RequireReceiver);
			QueryRopeInfo();
		}

		private void QueryRopeInfo()
		{
			RopeInfo r_info = new RopeInfo { joints = joints, needle_first = needle_first, needle_last = needle_last };
			SendMessage("RopeUpdate", r_info, SendMessageOptions.DontRequireReceiver);
		}

		private void CheckRopeSegments()
		{
			int segmentCount = Mathf.CeilToInt(ropeLength * segmentsPerLength);

			while(transform.childCount < segmentCount)
			{
				AddSegment();
			}
		}

		private void AddSegment()
		{
			int trueSegmentCount = transform.childCount;
			int middle = trueSegmentCount / 2;
			int next = middle + 1;

			GameObject toAddObj = Instantiate(segmentPrefab, transform);
			toAddObj.transform.SetSiblingIndex(next);

			RopeSegment toAddSeg = toAddObj.GetComponent<RopeSegment>();
			toAddSeg.Initialze(joints[next].GetComponent<RopeSegment>(), joints[middle].GetComponent<RopeSegment>());

			joints = GetComponentsInChildren<HingeJoint2D>();
		}

		private void RemoveSegment()
		{
			int trueSegmentCount = transform.childCount;
			int middle = trueSegmentCount / 2;
		}

		private void OnRopeCollisionEnter(Collision2D collision)
		{
			Collider2D other = collision.collider;

			// Check whether the detected object is a collectable.
			if (other.TryGetComponentInParent(out MovingCollectable collectable))
			{
				// TODO: Check whether the collectable is good or bad.
				if (true) // Good
				{
					// Increment point counter.
					// Add rope piece?
					// Add buff?
					// Destroy Collectable
					Destroy(collectable.gameObject);
				}
				else // Bad
				{
					// Take damage (split the rope)
					// End the game.
				}
			}
		}
	}
}
