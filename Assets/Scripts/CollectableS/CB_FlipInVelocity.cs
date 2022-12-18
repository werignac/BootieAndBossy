using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CB_FlipInVelocity : MonoBehaviour, CollectableBehaviour
{

	[SerializeField]
	private bool defaultRight;

	[SerializeField]
	private SpriteRenderer[] renderers;

	public void CollectableUpdate(Vector2 velocity)
	{
		bool isRight = Vector2.Dot(velocity, Vector2.right) >= 0;
		bool flip = defaultRight ^ isRight;
		SetFlipped(flip);
	}

	private void SetFlipped(bool flip)
	{
		foreach(SpriteRenderer sp in renderers)
			sp.flipX = flip;
	}
}
