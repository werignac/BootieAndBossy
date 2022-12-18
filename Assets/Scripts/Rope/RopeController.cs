using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using werignac.Utils;

namespace werignac.Rope
{
	struct RopeInfo { public HingeJoint2D[] joints; public GameObject needle_first; public GameObject needle_last; public int cut; }

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

		private float lastSegmentLength = 0;

		[SerializeField, Range(0.01f, 1)]
		private float lengthAddedPerCollectable = 0.25f;

		private int cut = -1;

		/////////////////////////
		//	Between-Frame Data //
		/////////////////////////

		/// <summary>
		/// Need to keep track of collectables already collected between Fixed
		/// Updates because multiple rope segment can collide with an object at
		/// the same time. Cleared on FixedUpdate, filled on collisions.
		/// </summary>
		private HashSet<GameObject> collectedCollectables = new HashSet<GameObject>();


		private void Start()
		{
			joints = GetComponentsInChildren<HingeJoint2D>();
		}

		private void Update()
		{
			int segmentCount = Mathf.CeilToInt(ropeLength * segmentsPerLength);
			float segmentLength = ropeLength / segmentCount;

			CheckRopeSegments();
			if (segmentLength != lastSegmentLength)
			{
				BroadcastMessage("SetTargetLength", 1 / segmentsPerLength, SendMessageOptions.RequireReceiver);
				lastSegmentLength = segmentLength;
			}
			QueryRopeInfo();
		}

		private void QueryRopeInfo()
		{
			RopeInfo r_info = new RopeInfo { joints = joints, needle_first = needle_first, needle_last = needle_last, cut = cut };
			SendMessage("RopeUpdate", r_info, SendMessageOptions.DontRequireReceiver);
		}

		private void CheckRopeSegments()
		{
			int segmentCount = Mathf.CeilToInt(ropeLength * segmentsPerLength);

			while(transform.childCount < segmentCount)
			{
				AddSegment();
			}

			while(transform.childCount > segmentCount)
			{
				RemoveSegment();
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

			transform.GetChild(middle).GetComponent<RopeSegment>().Merge();
			joints = GetComponentsInChildren<HingeJoint2D>();
		}

		private void FixedUpdate()
		{
			collectedCollectables.Clear();
		}

		private void OnRopeCollisionEnter(Collision2D collision)
		{
			Collider2D other = collision.collider;

			// Check whether the detected object is a collectable.
			if (other.TryGetComponentInParent(out MovingCollectable collectable))
			{
				bool alreadyCollected = collectedCollectables.Contains(collectable.gameObject);
				if (!alreadyCollected)
				{
					collectedCollectables.Add(collectable.gameObject);

					// TODO: Check whether the collectable is good or bad.
					if (true) // Good
					{
						// Increment point counter.
						ropeLength += lengthAddedPerCollectable;
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
}
