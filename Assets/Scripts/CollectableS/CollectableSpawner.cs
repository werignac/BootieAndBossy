using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoundsManager))]
public class CollectableSpawner : MonoBehaviour
{
	private BoundsManager boundsManager;

	private float nextSpawn;
	private float nextBadSpawn;
	[SerializeField, Min(0)]
	private float spawnTime = 5f;
	[SerializeField, Range(0f, 90f)]
	private float badSpawnTime = 30f;

	[SerializeField]
	private GameObject[] goodCollectables;
	[SerializeField]
	private GameObject[] badCollectables;

	[SerializeField, Range(0, 1)]
	private float badOdds = 0.333f;

	private List<GameObject> activeGoodCollectibles = new List<GameObject>();
	private List<GameObject> activeBadCollectibles = new List<GameObject>();

	private int goodCollectiblesSpawned = 1;
	private int badCollectiblesSpawned = 1;

	[SerializeField, Range(1, 20)]
	private int maxGoodCollectibles = 9;
	[SerializeField, Range(1, 10)]
	private int maxBadCollectibles = 4;

	[SerializeField]
	private GameObject warningPrefab;

    // Start is called before the first frame update
    void Start()
    {
		boundsManager = GetComponent<BoundsManager>();

		nextSpawn = Time.time + spawnTime;
		nextBadSpawn = Time.time + badSpawnTime;
    }

	private void Update()
	{
		if (Time.time > nextSpawn && maxGoodCollectibles > activeGoodCollectibles.Count)
		{
			GameObject collectible = InstantiateGoodCollectable();
			activeGoodCollectibles.Add(collectible);
			collectible.GetComponent<MovingCollectable>().onDestroy.AddListener(() => { 
				activeGoodCollectibles.Remove(collectible); 
			});
		}

		if (Time.time > nextBadSpawn && maxBadCollectibles > activeBadCollectibles.Count)
		{
			StartCoroutine(SpawnBadCollectibleCoroutine());
		}
	}

	private GameObject InstantiateGoodCollectable()
	{
		int collectableTypeCount = goodCollectables.Length;
		nextSpawn = Time.time + spawnTime / Mathf.Sqrt(goodCollectiblesSpawned++);
		return InstantiateCollectable(false, Mathf.Min((int)(collectableTypeCount * Random.value), collectableTypeCount));
	}

	private GameObject InstantiateBadCollectable()
	{
		int collectableTypeCount = badCollectables.Length;
		nextBadSpawn = Time.time + badSpawnTime / Mathf.Sqrt(badCollectiblesSpawned++);
		return InstantiateCollectable(true, Mathf.Min((int)(collectableTypeCount * Random.value), collectableTypeCount));
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
		Vector2 spawnPoint = boundsManager.GetRandomPointOnPerimeter(out Vector2 _);
		// TODO: Get max length of prefab To calc offset from camera.

		return Instantiate(spawnPrefab, spawnPoint, Quaternion.identity);
	}

	private IEnumerator SpawnBadCollectibleCoroutine()
	{
		nextBadSpawn = Time.time + badSpawnTime / Mathf.Sqrt(badCollectiblesSpawned++);

		// Temporarily place an entry to prevent multiple spawns during wait.
		int tempIndex = activeBadCollectibles.Count;
		activeBadCollectibles.Add(null);

		Vector2 spawnPoint = boundsManager.GetRandomPointOnPerimeter(out Vector2 normal);

		Instantiate(warningPrefab, spawnPoint + normal, Quaternion.identity);

		yield return new WaitForSeconds(2.5f);

		int badCount = badCollectables.Length;

		GameObject prefab = badCollectables[Random.Range(0, badCount)];

		GameObject collectible = Instantiate(prefab, spawnPoint, Quaternion.identity);

		activeBadCollectibles.RemoveAt(tempIndex);
		activeBadCollectibles.Add(collectible);
		
		collectible.GetComponent<MovingCollectable>().onDestroy.AddListener(() => {
			activeBadCollectibles.Remove(collectible);
		});
	}
}
