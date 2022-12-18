using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoundsManager))]
public class CollectableSpawner : MonoBehaviour
{
	private BoundsManager boundsManager;

	private float nextSpawn;
	[SerializeField, Min(0)]
	private float spawnTime = 5f;

	[SerializeField]
	private GameObject[] goodCollectables;
	[SerializeField]
	private GameObject[] badCollectables;

	[SerializeField, Range(0, 1)]
	private float badOdds = 0.333f;

    // Start is called before the first frame update
    void Start()
    {
		boundsManager = GetComponent<BoundsManager>();

		nextSpawn = Time.time + spawnTime;
    }

	private void Update()
	{
		if (Time.time > nextSpawn)
		{
			InstantiateCollectable();
		}
	}

	private GameObject InstantiateCollectable()
	{
		bool isBad = Random.value < badOdds;
		int collectableTypeCount;

		if (!isBad)
			collectableTypeCount = goodCollectables.Length;
		else
			collectableTypeCount = badCollectables.Length;

		return InstantiateCollectable(isBad, Mathf.Min((int)(collectableTypeCount * Random.value), collectableTypeCount));
	}

	private GameObject InstantiateCollectable(bool isBad, int collectableIndex)
	{
		GameObject prefab;

		if (!isBad)
			prefab = goodCollectables[collectableIndex];
		else
			prefab = badCollectables[collectableIndex];

		return InstantiateCollectable(prefab);
	}

	private GameObject InstantiateCollectable(GameObject spawnPrefab)
	{
		Vector2 spawnPoint = boundsManager.GetRandomPointOnPerimeter();
		// TODO: Get max length of prefab To calc offset from camera.
		nextSpawn = Time.time + spawnTime;

		return Instantiate(spawnPrefab, spawnPoint, Quaternion.identity);
		// TODO: Set movement direction here.
	}
}
