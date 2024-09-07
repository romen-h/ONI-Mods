using System.Collections;
using System.Collections.Generic;
using System.Reflection;

using UnityEngine;

namespace RomenH.Common
{
	public class RomenHRegistry : MonoBehaviour, IDictionary<string, object>
	{
		private Dictionary<string, object> registry = new Dictionary<string, object>();

		public static IDictionary<string, object> Init()
		{
			Debug.Log($"[RomenH.Registry] Initializing from {Assembly.GetCallingAssembly().GetName().Name}");

			var go = Global.Instance.gameObject;
			if (go == null)
			{
				Debug.LogWarning("[RomenH.Registry] Could not acquire Global GameObject.");
				return null;
			}

			var reg = go.GetComponent(nameof(RomenHRegistry));
			if (reg == null)
			{
				Debug.Log("[RomenH.Registry] Creating new registry.");
				reg = go.AddComponent<RomenHRegistry>();
			}
			else
			{
				Debug.Log("[RomenH.Registry] Found existing registry.");
			}

			if (reg is IDictionary<string, object> ret)
			{
				return ret;
			}

			Debug.LogWarning("[RomenH.Registry] Failed to acquire registry. Returning placeholder registry.");
			return new Dictionary<string, object>();
		}

		#region IDictionary

		public ICollection<string> Keys => registry.Keys;

		public ICollection<object> Values => registry.Values;

		public int Count => registry.Count;

		public bool IsReadOnly => ((ICollection<KeyValuePair<string, object>>)registry).IsReadOnly;

		public object this[string key]
		{
			get => registry[key];
			set => registry[key] = value;
		}

		public bool ContainsKey(string key)
		{
			return registry.ContainsKey(key);
		}

		public void Add(string key, object value)
		{
			registry.Add(key, value);
		}

		public bool Remove(string key)
		{
			return registry.Remove(key);
		}

		public bool TryGetValue(string key, out object value)
		{
			return registry.TryGetValue(key, out value);
		}

		public void Clear()
		{
			registry.Clear();
		}

		public void Add(KeyValuePair<string, object> item)
		{
			((ICollection<KeyValuePair<string, object>>)registry).Add(item);
		}

		public bool Contains(KeyValuePair<string, object> item)
		{
			return ((ICollection<KeyValuePair<string, object>>)registry).Contains(item);
		}

		public void CopyTo(KeyValuePair<string, object>[] array, int arrayIndex)
		{
			((ICollection<KeyValuePair<string, object>>)registry).CopyTo(array, arrayIndex);
		}

		public bool Remove(KeyValuePair<string, object> item)
		{
			return ((ICollection<KeyValuePair<string, object>>)registry).Remove(item);
		}

		public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
		{
			return registry.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return registry.GetEnumerator();
		}

		#endregion
	}
}
