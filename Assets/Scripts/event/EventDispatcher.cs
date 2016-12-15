using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;
using System.Reflection;
using System;
using UnityEngine.Assertions;

namespace max.events{
	public class EventDispatcher : MonoBehaviour, IEventDispatcher {
		
		private static List<List<Action<string, object>>> cacheList = new List<List<Action<string, object>>>();
		private Dictionary<string, List<Action<string, object>>> eventMap;

        /// <summary>
        /// Awake is called when the script instance is being loaded.
        /// </summary>
        void Awake()
		{
			eventMap = new Dictionary<string, List<Action<string, object>>>();
		}

        public void Add(string type, Action<string, object> listener)
        {
			List<Action<string, object>> list;
			if (!eventMap.TryGetValue(type, out list))
			{
				list = AcquireList();
				eventMap.Add(type, list);
			}
			
			bool exists = list.Contains(listener);
			Assert.IsTrue(exists);
			if (!exists)
			{
				list.Add(listener);
			}
        }

        public void Dispatch(string type, object data)
        {
			List<Action<string, object>> list;
			if (eventMap.TryGetValue(type, out list))
			{
				int len = list.Count;
				for (int i = 0; i < len; i++)
				{
					list[i].Invoke(type, data);
				}
			}
        }

        public bool Has(string type)
        {
			return eventMap.ContainsKey(type);
        }

        public void Remove(string type, Action<string, object> listener)
        {
			List<Action<string, object>> list;
			if (eventMap.TryGetValue(type, out list))
			{
				list.Remove(listener);
			}
        }

		List<Action<string, object>> AcquireList()
		{
			int len = cacheList.Count;
			if (len > 0)
			{
				var item = cacheList[0];
				cacheList.RemoveAt(0);
				return item;
			}
			return new List<Action<string, object>>();
		}

		void CacheList(List<Action<string, object>> toCache)
		{
			toCache.Clear();
			cacheList.Add(toCache);
		}

		/*void BubbleEvent(string type, object data)
		{
			List<EventDispatcher> chain = new List<EventDispatcher>();
			while (transform.parent.GetType() == typeof(EventDispatcher))
			{
				chain.Add(this);
			}
		}*/

        /// <summary>
        /// This function is called when the MonoBehaviour will be destroyed.
        /// </summary>
        void OnDestroy()
		{
			foreach (KeyValuePair<string, List<Action<string, object>>> item in eventMap)
			{
				CacheList(item.Value);
			}
			eventMap.Clear();
		}
    }
}

public interface IEventDispatcher
{
	void Add(string type, Action<string, object> listener);
	void Dispatch(string type, object data);
	void Remove(string type, Action<string, object> listener);
	bool Has(string type);
}