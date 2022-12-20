using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using werignac.Curves;


namespace werignac.Rope
{
	[RequireComponent(typeof(RopeController))]
	public class RopeVisualizer : MonoBehaviour
	{
		/// <summary>
		/// Primary line renderer. Used for rendering the rope most
		/// of the time.
		/// </summary>
		LineRenderer p_lineRenderer;
		/// <summary>
		/// Secondary line renderer, Used for rendering the rope when
		/// cut.
		/// </summary>
		LineRenderer s_lineRenderer;

		[SerializeField]
		private int resolution = 10;
		
		enum RopeSpline {BEZIER, CATMULL_ROM, MIXED}

		[SerializeField]
		private RopeSpline v_spline = RopeSpline.BEZIER;

		private int PositionCount (HingeJoint2D[] joints) { return resolution * joints.Length; }

		// Start is called before the first frame update
		void Start()
		{
			LineRenderer[] renderers = GetComponentsInChildren<LineRenderer>();

			p_lineRenderer = renderers[0];
			s_lineRenderer = renderers[1];
			s_lineRenderer.enabled = false;
		}

		private void RopeUpdate(RopeInfo info)
		{
			// Split info into two infos if there is a cut.
			if (info.cut < 0)
			{
				SetLineRendererFromInfo(p_lineRenderer, info);
				s_lineRenderer.enabled = false;
			}
			else
			{
				List<HingeJoint2D> p_joints = new List<HingeJoint2D>();
				List<HingeJoint2D> s_joints = new List<HingeJoint2D>();

				for (int i = 0; i < info.joints.Length; i++)
				{
					HingeJoint2D joint = info.joints[i];
					if (i < info.cut)
						p_joints.Add(joint);
					else
						s_joints.Add(joint);
				}

				RopeInfo p_info = new RopeInfo { joints = p_joints.ToArray(), needle_first = info.needle_first, needle_last = info.needle_last, cut = -1 };
				RopeInfo s_info = new RopeInfo { joints = s_joints.ToArray(), needle_first = info.needle_first, needle_last = info.needle_last, cut = -1 };

				if (p_info.joints.Length > 0)
				{
					p_lineRenderer.enabled = true;
					SetLineRendererFromInfo(p_lineRenderer, p_info);
				}
				else
					p_lineRenderer.enabled = false;

				if (s_info.joints.Length > 0)
				{
					s_lineRenderer.enabled = true;
					SetLineRendererFromInfo(s_lineRenderer, s_info);
				}
				else
					s_lineRenderer.enabled = false;
			}

		}

		private void SetLineRendererFromInfo(LineRenderer lineRenderer, RopeInfo info)
		{
			int positionCount = PositionCount(info.joints);

			lineRenderer.positionCount = positionCount;


			Vector3[] positions = new Vector3[positionCount];

			Curve curve = null;

#if UNITY_EDITOR
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
#else
			curve = new MultiCatmullRom(GenerateControlPointsForCatmullRom(info));
#endif
			for (int i = 0; i < positionCount; i++)
				positions[i] = curve.Evaluate((float)i / positionCount);

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
