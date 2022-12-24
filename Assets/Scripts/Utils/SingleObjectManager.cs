using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleObjectManager : MonoBehaviour
{
	[SerializeField]
	private GameObject prefab;
	private GameObject activeObj = null;

    public void InstantiateActiveObj()
	{
		if (activeObj == null)
			activeObj = Instantiate(prefab, transform);
	}

	public void DestroyActiveObject()
	{
		if (activeObj != null)
		{
			Destroy(activeObj);
			activeObj = null;
		}
	}
}
