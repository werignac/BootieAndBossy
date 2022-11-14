using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class RopeController : MonoBehaviour
{
	HingeJoint2D[] joints;
	LineRenderer lineRenderer;

	[SerializeField]
	private int resolution = 10;
	[SerializeField]
	private GameObject needle_first;
	[SerializeField]
	private GameObject needle_last;

	private abstract class CurveFormulation
	{
		public abstract Vector3 Evaluate(float t);
	}

	private abstract class Curve : CurveFormulation
	{
		protected Vector3[] controlPoints;
		public Curve(Vector3[] _controlPoints)
		{
			controlPoints = _controlPoints;
		}
	}

	private class Bezier : Curve
	{
		
		float[] coeffecients;

		public Bezier(Vector3[] _controlPoints) : base(_controlPoints)
		{
			coeffecients = new float[_controlPoints.Length];
			for (int i = 0; i < _controlPoints.Length; i++)
				coeffecients[i] = Pascale(_controlPoints.Length - 1, i);
		}

		private float Factorial(int n)
		{
			if (n == 0)
				return 1;

			float factorial = n;
			for (int i = n - 1; i > 0; i--)
				factorial *= i;
			return factorial;
		}

		private float Pascale(int n, int r)
		{
			return Factorial(n) / (Factorial(r) * Factorial(n - r));
		}

		public override Vector3 Evaluate(float t)
		{
			Vector3 point = Vector3.zero;
			int n = controlPoints.Length - 1;

			for (int r = 0; r < controlPoints.Length; r++)
			{
				Vector3 controlPoint = controlPoints[r];

				float k = coeffecients[r];

				point += k * Mathf.Pow(1 - t, n - r) * Mathf.Pow(t, r) * controlPoint;
			}

			return point;
		}
	}

	private class CatmullRom : Curve
	{
		float alpha = 1f;
		float tension = 0f;

		Vector3 a;
		Vector3 b;
		Vector3 c;
		Vector3 d;

		public CatmullRom(Vector3[] _controlPoints) : base(_controlPoints)
		{
			Vector3 p0 = _controlPoints[0];
			Vector3 p1 = _controlPoints[1];
			Vector3 p2 = _controlPoints[2];
			Vector3 p3 = _controlPoints[3];

			float t01 = Mathf.Pow(Vector3.Distance(p0, p1), alpha);
			float t12 = Mathf.Pow(Vector3.Distance(p1, p2), alpha);
			float t23 = Mathf.Pow(Vector3.Distance(p2, p3), alpha);

			Vector3 m1 = (1 - tension) * (p2 - p1 + t12 * ((p1 - p0) / t01 - (p1 - p0) / (t01 + t12)));
			Vector3 m2 = (1 - tension) * (p2 - p1 + t12 * ((p3 - p2) / t23 - (p3 - p1) / (t12 + t23)));

			a = 2.0f * (p1 - p2) + m1 + m2;
			b = -3.0f * (p1 - p2) - m1 - m1 - m2;
			c = m1;
			d = p1;
		}

		public override Vector3 Evaluate(float t)
		{
			return a * Mathf.Pow(t, 3) + b * Mathf.Pow(t, 2) + c * t + d;
		}
	}

	private class MultiCatmullRom : CurveFormulation
	{
		CatmullRom[] curves;

		public MultiCatmullRom(Vector3[] _controlPoints)
		{
			curves = new CatmullRom[_controlPoints.Length - 3];
			
			for (int i  = 1; i < _controlPoints.Length - 2; i++)
				curves[i - 1] = new CatmullRom(new Vector3[] { _controlPoints[i - 1], _controlPoints[i], _controlPoints[i + 1], _controlPoints[i + 2] });
		}

		public override Vector3 Evaluate(float t)
		{
			int index = Mathf.Min(Mathf.FloorToInt(t * curves.Length), curves.Length - 1);
			float percent = t * curves.Length - index;

			return curves[index].Evaluate(percent);
		}

	}

	private class InterpolationCurve : CurveFormulation
	{
		CurveFormulation curve1;
		CurveFormulation curve2;

		public InterpolationCurve(CurveFormulation _c1, CurveFormulation _c2)
		{
			curve1 = _c1;
			curve2 = _c2;
		}

		public override Vector3 Evaluate(float t)
		{
			return Vector3.Lerp(curve1.Evaluate(t), curve2.Evaluate(t), 0.5f);
		}
	}

	private int PositionCount { get { return resolution * joints.Length; } }

    // Start is called before the first frame update
    void Start()
    {
		joints = GetComponentsInChildren<HingeJoint2D>();
		lineRenderer = GetComponent<LineRenderer>();
		lineRenderer.positionCount = PositionCount;
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

    // Update is called once per frame
    void Update()
    {
#if UNITY_EDITOR
		

		Vector3[] positions = new Vector3[PositionCount];

		Curve curve1 = new Bezier(GenerateControlPointsForBezier());
		CurveFormulation curve2 = new MultiCatmullRom(GenerateControlPointsForCatmullRom());
		CurveFormulation curve3 = new InterpolationCurve(curve1, curve2);

		for (int i = 0; i < PositionCount; i++)
			positions[i] = curve3.Evaluate((float)i / PositionCount);
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

	private Vector3[] GenerateControlPointsForBezier()
	{
		Vector3[] controlPoints_bz = new Vector3[joints.Length];

		for (int i = 0; i < joints.Length; i++)
			controlPoints_bz[i] = GetJointWorldPosition(joints[i]);

		return controlPoints_bz;
	}

	private Vector3[] GenerateControlPointsForCatmullRom()
	{
		Vector3[] controlPoints_cr = new Vector3[joints.Length + 2];

		controlPoints_cr[0] = needle_first.transform.up + (Vector3)GetJointWorldPosition(joints[0]);
		for (int i = 1; i <= joints.Length; i++)
			controlPoints_cr[i] = GetJointWorldPosition(joints[i - 1]);
		controlPoints_cr[joints.Length + 1] = needle_last.transform.up + (Vector3)GetJointWorldPosition(joints[joints.Length - 1]);

		return controlPoints_cr;
	}
}
