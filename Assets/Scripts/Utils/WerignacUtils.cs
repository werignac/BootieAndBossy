using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
	}
}
