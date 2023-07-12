using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace werignac.Utils
{
	public static class WerignacUtils
	{
		#region TryGetComponentInParent
		public static bool TryGetComponentInParent<T>(this Component inComponent, out T outComponent)
		{
			outComponent = inComponent.GetComponentInParent<T>();
			return outComponent != null;
		}
		
		public static bool TryGetComponentInParent<T>(this GameObject gameObject, out T component)
		{
			component = gameObject.GetComponentInParent<T>();
			return component != null;
		}
		#endregion

		#region BroadcastToAll
		public static void BroadcastToAll(string methodName, object parameter = null)
		{
			foreach(GameObject root in SceneManager.GetActiveScene().GetRootGameObjects())
				root.BroadcastMessage(methodName, parameter, SendMessageOptions.DontRequireReceiver);
		}
		#endregion

		#region PercolateUp

		public class Percolation<T>
		{
			private bool halt = false;
			private T data;

			public Percolation( T _data)
			{
				data = _data;
			}

			public T GetData()
			{
				return data;
			}

			public void Halt()
			{
				halt = true;
			}

			public bool GetHalt()
			{
				return halt;
			}
		}

		public static void Percolate<T>(this GameObject obj,string recieverFunction, T data)
		{
			GameObject current = obj.transform.parent.gameObject;
			Percolation<T> percolation = new Percolation<T>(data);

			while(current && ! percolation.GetHalt())
			{
				current.SendMessage(recieverFunction, percolation, SendMessageOptions.DontRequireReceiver);
				current = current.transform.parent.gameObject;
			}
		}
		
		#endregion

		// TODO: Add a query class
	}
}
