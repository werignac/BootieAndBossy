using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace werignac.Curves
{
	abstract class Curve
	{
		public abstract Vector3 Evaluate(float t);
	}

	class Bezier : Curve
	{

		Vector3[] coeffecients;

		public Bezier(Vector3[] _controlPoints)
		{
			coeffecients = new Vector3[_controlPoints.Length];
			for (int i = 0; i < _controlPoints.Length; i++)
				coeffecients[i] = Pascale(_controlPoints.Length - 1, i) * _controlPoints[i];
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
			int n = coeffecients.Length - 1;

			for (int r = 0; r < coeffecients.Length; r++)
			{
				Vector3 k = coeffecients[r];
				point += k * Mathf.Pow(1 - t, n - r) * Mathf.Pow(t, r);
			}

			return point;
		}
	}

	class CatmullRom : Curve
	{
		float alpha = 1f;
		float tension = 0f;

		Vector3 a;
		Vector3 b;
		Vector3 c;
		Vector3 d;

		public CatmullRom(Vector3[] _controlPoints)
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

	class MultiCatmullRom : Curve
	{
		CatmullRom[] curves;

		public MultiCatmullRom(Vector3[] _controlPoints)
		{
			curves = new CatmullRom[_controlPoints.Length - 3];

			for (int i = 1; i < _controlPoints.Length - 2; i++)
				curves[i - 1] = new CatmullRom(new Vector3[] { _controlPoints[i - 1], _controlPoints[i], _controlPoints[i + 1], _controlPoints[i + 2] });
		}

		public override Vector3 Evaluate(float t)
		{
			int index = Mathf.Min(Mathf.FloorToInt(t * curves.Length), curves.Length - 1);
			float percent = t * curves.Length - index;

			return curves[index].Evaluate(percent);
		}

	}

	class InterpolationCurve : Curve
	{
		Curve curve1;
		Curve curve2;

		public InterpolationCurve(Curve _c1, Curve _c2)
		{
			curve1 = _c1;
			curve2 = _c2;
		}

		public override Vector3 Evaluate(float t)
		{
			return Vector3.Lerp(curve1.Evaluate(t), curve2.Evaluate(t), 0.5f);
		}
	}
}
