using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PolygonCollider2D))]
public class BoundsManager : MonoBehaviour
{
	[SerializeField]
	private float margin = 1;
	private PolygonCollider2D hardBounds;
	private BoxCollider2D triggerBounds;
	private Vector2 p1;
	private Vector2 p2;
	private Vector2 p3;
	private Vector2 p4;

	private int screenWidth = 0;
	private int screenHeight = 0;

	private float Perimeter { get { return 2 * Vector2.Distance(p1, p2) + 2 * Vector2.Distance(p2, p3); } }

    // Start is called before the first frame update
    void Start()
    {
        hardBounds = GetComponent<PolygonCollider2D>();
		triggerBounds = GetComponentInChildren<BoxCollider2D>();
		screenWidth = Screen.width;
		screenHeight = Screen.height;
		CreateMargin();
    }

	private void Update()
	{
		if (Screen.width != screenWidth || Screen.height != screenHeight)
		{
			screenWidth = Screen.width;
			screenHeight = Screen.height;
			CreateMargin();
		}
	}


	private void CreateMargin()
	{
		Camera camera = Camera.main;
		p1 = camera.ScreenToWorldPoint(new Vector3(0, 0));
		p2 = camera.ScreenToWorldPoint(new Vector3(camera.pixelWidth, 0));
		p3 = camera.ScreenToWorldPoint(new Vector3(camera.pixelWidth, camera.pixelHeight));
		p4 = camera.ScreenToWorldPoint(new Vector3(0, camera.pixelHeight));

		hardBounds.pathCount = 2;
		hardBounds.SetPath(0, new Vector2[] { p1, p2, p3, p4 });
		hardBounds.SetPath(1, new Vector2[] { p1 + new Vector2(-margin, -margin), p2 + new Vector2(margin, -margin), p3 + new Vector2(margin, margin), p4 + new Vector2(-margin, margin) });

		triggerBounds.size = new Vector2(p2.x - p1.x, p3.y - p2.y);
	}

	public Vector2 GetRandomPointOnPerimeter()
	{
		return GetPointOnPerimeter(Random.value);
	}

	private Vector2 GetPointOnPerimeter(float t)
	{
		Vector2[] startPoints = new Vector2[] { p1, p2, p3, p4 };
		Vector2[] endPoints = new Vector2[] { p2, p3, p4, p1 };

		float perimeterTraversal = 0f;

		Vector2 startPoint = Vector2.zero;
		Vector2 endPoint = Vector2.zero;
		float edgeLength = 0f;

		for (int i = 0; i < 4; i++)
		{
			startPoint = startPoints[i];
			endPoint = endPoints[i];
			edgeLength = Vector2.Distance(startPoint, endPoint);
			perimeterTraversal += edgeLength;

			if (perimeterTraversal / Perimeter >= t)
				break;
		}

		float t_onEdge = (t - (perimeterTraversal - edgeLength) / Perimeter) * (Perimeter / edgeLength);
		return Vector2.Lerp(startPoint, endPoint, t_onEdge);
	}

	public Vector2 ClosetsPointOnBounds(Vector2 worldPos)
	{
		return hardBounds.ClosestPoint(worldPos);
	}
}
