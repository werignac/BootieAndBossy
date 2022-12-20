using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetachFromParent : MonoBehaviour
{
    public void Detach()
	{
		transform.parent = null;
	}
}
