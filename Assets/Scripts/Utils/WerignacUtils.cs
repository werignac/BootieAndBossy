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
	}
}
