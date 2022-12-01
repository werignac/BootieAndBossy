using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using werignac.Curves;


namespace werignac.Rope
{
	[RequireComponent(typeof(LineRenderer), typeof(RopeController))]
	public class RopeVisualizer : MonoBehaviour
	{
		LineRenderer lineRenderer;

		[SerializeField]
		private int resolution = 10;
		
		enum RopeSpline {BEZIER, CATMULL_ROM, MIXED}

		[SerializeField]
		private RopeSpline v_spline = RopeSpline.BEZIER;

		private int PositionCount (HingeJoint2D[] joints) { return resolution * joints.Length; }

		// Start is called before the first frame update
		void Start()
		{
			lineRenderer = GetComponent<LineRenderer>();
		}

		private void RopeUpdate(RopeInfo info)
		{
			int positionCount = PositionCount(info.joints);

			lineRenderer.positionCount = positionCount;

#if UNITY_EDITOR
			Vector3[] positions = new Vector3[positionCount];

			Curve curve = null;

			switch (v_spline)
			{
				case RopeSpline.BEZIER:
					curve = new Bezier(GenerateControlPointsForBezier(info));
					break;
				case RopeSpline.CATMULL_ROM:
					curve = new MultiCatmullRom(GenerateControlPointsForCatmullRom(info));
					break;
				case RopeSpline.MIXED:
					Curve bezier = new Bezier(GenerateControlPointsForBezier(info));
					Curve catmullRom = new MultiCatmullRom(GenerateControlPointsForCatmullRom(info));
					curve = new InterpolationCurve(bezier, catmullRom);
					break;
			}

			for (int i = 0; i < positionCount; i++)
				positions[i] = curve.Evaluate((float)i / positionCount);
#else
		Vector3[] controlPoints = new Vector3[joints.Length];

		for (int i = 0; i < joints.Length; i++)
			controlPoints[i] = GetJointWorldPosition(joints[i]);

		Vector3[] positions = new Vector3[PositionCount];

		Curve bezier = new MultiCatmullRom(controlPoints);

		for (int i = 0; i < PositionCount; i++)
			positions[i] = bezier.Evaluate((float)i / PositionCount);
#endif
			lineRenderer.SetPositions(positions);
		}

		Vector2 GetJointWorldPosition(HingeJoint2D joint)
		{
			return joint.transform.TransformPoint(joint.anchor);
		}

		IEnumerator<Vector2> GetJointWorldPositions(HingeJoint2D[] joints)
		{
			for (int i = 0; i < joints.Length; i++)
				yield return GetJointWorldPosition(joints[i]);
		}

		private Vector3[] GenerateControlPointsForBezier(RopeInfo info)
		{
			HingeJoint2D[] joints = info.joints;

			Vector3[] controlPoints_bz = new Vector3[joints.Length];

			for (int i = 0; i < joints.Length; i++)
				controlPoints_bz[i] = GetJointWorldPosition(joints[i]);

			return controlPoints_bz;
		}

		private Vector3[] GenerateControlPointsForCatmullRom(RopeInfo info)
		{
			HingeJoint2D[] joints = info.joints;
			GameObject needle_first = info.needle_first;
			GameObject needle_last = info.needle_last;

			Vector3[] controlPoints_cr = new Vector3[joints.Length + 2];

			controlPoints_cr[0] = needle_first.transform.up + (Vector3)GetJointWorldPosition(joints[0]);
			for (int i = 1; i <= joints.Length; i++)
				controlPoints_cr[i] = GetJointWorldPosition(joints[i - 1]);
			controlPoints_cr[joints.Length + 1] = needle_last.transform.up + (Vector3)GetJointWorldPosition(joints[joints.Length - 1]);

			return controlPoints_cr;
		}
	}
}
