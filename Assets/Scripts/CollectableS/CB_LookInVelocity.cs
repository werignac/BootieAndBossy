using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CB_LookInVelocity : MonoBehaviour, CollectableBehaviour
{
	public void CollectableUpdate(Vector2 velocity)
	{
		transform.rotation = Quaternion.LookRotation(Vector3.forward, Quaternion.Euler(0, 0, 90f) * velocity);
	}
}
