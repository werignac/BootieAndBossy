using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using werignac.Utils;
using System;

namespace werignac.Rope
{
	struct RopeInfo { public HingeJoint2D[] joints; public GameObject needle_first; public GameObject needle_last; public int cut; }

	public class RopeController : MonoBehaviour
	{
		//////////////////////////////////////////////////
		//  Rope Objects (Kneedles and Discretization)  //
		//////////////////////////////////////////////////
		private HingeJoint2D[] joints;

		[SerializeField]
		private GameObject segmentPrefab;

		[SerializeField]
		private GameObject needle_first;
		[SerializeField]
		private GameObject needle_last;

		[SerializeField]
		private Transform segmentParent;

		////////////////////////////////////////
		//  Rope Properties (Length Control)  //
		////////////////////////////////////////
		[SerializeField, Range(0.5f, 10)]
		private float ropeLength = 4f;

		[SerializeField, Range(1, 10)]
		private float segmentsPerLength = 1;

		private float lastSegmentLength = 0;

		[SerializeField, Range(0.01f, 1)]
		private float lengthAddedPerCollectable = 0.25f;

		/// <summary>
		/// Cur indicates where the rope is cut.
		/// </summary>
		private int cut = -1;

		private bool IsCut { get { return cut >= 0; } }

		/////////////////////////
		//  Between-Step Data  //
		/////////////////////////

		/// <summary>
		/// Need to keep track of collectables already collected between Fixed
		/// Updates because multiple rope segment can collide with an object at
		/// the same time. Cleared on FixedUpdate, filled on collisions.
		/// </summary>
		private HashSet<GameObject> collectedCollectables = new HashSet<GameObject>();


		private void Start()
		{
			joints = segmentParent.GetComponentsInChildren<HingeJoint2D>();
		}

		private void Update()
		{
			if (!IsCut)
			{

				int segmentCount = Mathf.CeilToInt(ropeLength * segmentsPerLength);
				float segmentLength = ropeLength / segmentCount;

				CheckRopeSegments();
				if (segmentLength != lastSegmentLength)
				{
					BroadcastMessage("SetTargetLength", 1 / segmentsPerLength, SendMessageOptions.RequireReceiver);
					lastSegmentLength = segmentLength;
				}
			}

			QueryRopeInfo();
		}

		private void QueryRopeInfo()
		{
			joints = segmentParent.GetComponentsInChildren<HingeJoint2D>();

			RopeInfo r_info = new RopeInfo { joints = joints, needle_first = needle_first, needle_last = needle_last, cut = cut };
			SendMessage("RopeUpdate", r_info, SendMessageOptions.DontRequireReceiver);
		}

		private void CheckRopeSegments()
		{
			int segmentCount = Mathf.CeilToInt(ropeLength * segmentsPerLength);

			while(segmentParent.childCount < segmentCount)
			{
				AddSegment();
			}

			while(segmentParent.childCount > segmentCount)
			{
				RemoveSegment();
			}
		}

		private void AddSegment()
		{
			int trueSegmentCount = segmentParent.childCount;
			int middle = trueSegmentCount / 2;
			int next = middle + 1;

			GameObject toAddObj = Instantiate(segmentPrefab, segmentParent);
			toAddObj.transform.SetSiblingIndex(next);

			RopeSegment toAddSeg = toAddObj.GetComponent<RopeSegment>();
			toAddSeg.Initialze(joints[next].GetComponent<RopeSegment>(), joints[middle].GetComponent<RopeSegment>());

			joints = segmentParent.GetComponentsInChildren<HingeJoint2D>();
		}

		private void RemoveSegment()
		{
			int trueSegmentCount = segmentParent.childCount;
			int middle = trueSegmentCount / 2;

			segmentParent.GetChild(middle).GetComponent<RopeSegment>().Merge();
			joints = segmentParent.GetComponentsInChildren<HingeJoint2D>();
		}

		private void Cut(RopeSegment segment)
		{
			cut = Array.IndexOf(joints, segment.connectorA);

			Destroy(segment.gameObject);

			joints = segmentParent.GetComponentsInChildren<HingeJoint2D>();
		}

		private void FixedUpdate()
		{
			// Reset Between-Step Data
			collectedCollectables.Clear();
		}

		private void OnRopeCollisionEnter(Collision2D collision)
		{
			Collider2D other = collision.collider;

			// Check whether the detected object is a collectable.
			if (other.TryGetComponentInParent(out MovingCollectable collectable) && !IsCut)
			{
				bool alreadyCollected = collectedCollectables.Contains(collectable.gameObject);
				if (!alreadyCollected)
				{
					collectedCollectables.Add(collectable.gameObject);

					// Check whether the collectable is good or bad.
					if (collectable.isGood) // Good
					{
						// Increment point counter.
						ropeLength += lengthAddedPerCollectable;
						// Destroy Collectable
						Destroy(collectable.gameObject);
						// Notify that a good Collectable was collected.
						WerignacUtils.BroadcastToAll("OnCollectableCollected");
					}
					else // Bad
					{
						// Cut the rope.
						Cut(collision.otherCollider.GetComponentInParent<RopeSegment>());
						// End the game.
						WerignacUtils.BroadcastToAll("OnRopeCut");
					}
				}
			}
		}
	}
}
