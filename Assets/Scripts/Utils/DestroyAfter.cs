using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfter : MonoBehaviour
{
	[SerializeField, Min(0)]
	private float timeToDie = 1f;
	[SerializeField]
	private bool destroyOnStart = false;


	private void Start()
	{
		if (destroyOnStart)
			DestroyTimed();
	}

	public void DestroyTimed()
	{
		StartCoroutine(DestroyCoroutine());
	}

	private IEnumerator DestroyCoroutine()
	{
		yield return new WaitForSeconds(timeToDie);
		Destroy(gameObject);
	}
    
}
