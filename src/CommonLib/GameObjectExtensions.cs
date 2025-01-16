using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;

namespace RomenH.Common
{
	public static class GameObjectExtensions
	{
		public static GameObject GetInactiveChild(this GameObject gameObject, string name)
		{
			return null;
		}

		public static T SafeGetComponent<T>(this GameObject gameObject) where T : Component
		{
			if (gameObject == null) return null;
			if (gameObject.TryGetComponent<T>(out T cmp)) return cmp;
			return null;
		}
	}
}
