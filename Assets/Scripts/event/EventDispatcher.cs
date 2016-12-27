using UnityEngine;
using System.Collections.Generic;
using System.Reflection;
using System;
using UnityEngine.Assertions;
using max.events;

namespace max.events{
	public class EventDispatcher : MonoBehaviour, IEventDispatcher {
		
		private static List<List<Action<EEvent>>> cacheList = new List<List<Action<EEvent>>>();
		private Dictionary<string, List<Action<EEvent>>> eventMap;

        /// <summary>
        /// Awake is called when the script instance is being loaded.
        /// </summary>
        void Awake()
		{
			eventMap = new Dictionary<string, List<Action<EEvent>>>();
		}

        public void Add(string type, Action<EEvent> listener)
        {
			List<Action<EEvent>> list;
			if (!eventMap.TryGetValue(type, out list))
			{
				list = AcquireList();
				eventMap.Add(type, list);
			}
			
			bool exists = list.Contains(listener);
			Assert.IsFalse(exists);
			if (!exists)
			{
				list.Add(listener);
			}
        }

        /*public void Dispatch(string type, object data)
        {
			List<Action<EEvent>> list;
			if (eventMap.TryGetValue(type, out list))
			{
				int len = list.Count;
				for (int i = 0; i < len; i++)
				{
					list[i].Invoke(type, data);
				}
			}
        }*/

		public void Dispatch(EEvent evt)
		{
			bool bubbles = evt.bubbles;
			if (!bubbles && !eventMap.ContainsKey(evt.type)) return;	// If not bubbling and no listener added, no need to proceed.

			EventDispatcher previousTarget = evt.target;
			evt.SetTarget(this);

			if (bubbles) bubbleEvent(evt);
			else		 invokeEvent(evt);

			evt.SetTarget(previousTarget);
		}

        public bool Has(string type)
        {
			return eventMap.ContainsKey(type);
        }

        public void Remove(string type, Action<EEvent> listener)
        {
			List<Action<EEvent>> list;
			if (eventMap.TryGetValue(type, out list))
			{
				list.Remove(listener);
			}
        }

		private void bubbleEvent(EEvent evt)
		{
			Transform _transform = transform;
			List<EventDispatcher> chain = new List<EventDispatcher>();
			while (_transform != null)
			{
				EventDispatcher dispatcher = _transform.gameObject.GetComponent<EventDispatcher>();
				if (dispatcher == null)	break;
				chain.Add(dispatcher);
				_transform = _transform.parent;
			}

			for (int i = 0; i < chain.Count; i++)
			{
				bool stopPropagation = chain[i].invokeEvent(evt);
				if (stopPropagation) break;
			}
			chain.Clear();
		}

        private bool invokeEvent(EEvent evt)
        {
			string type = evt.type;
			List<Action<EEvent>> list;
			if (eventMap.TryGetValue(type, out list))
			{
				evt.SetCurrentTarget(this);
				int len = list.Count;
				for (int i = 0; i < len; i++)
				{
					list[i].Invoke(evt);
					if (evt.stopImmediatePropagation) return true;
				}

				return evt.stopPropagation;
			}
			return false;
        }

		List<Action<EEvent>> AcquireList()
		{
			int len = cacheList.Count;
			if (len > 0)
			{
				var item = cacheList[0];
				cacheList.RemoveAt(0);
				return item;
			}
			return new List<Action<EEvent>>();
		}

		void CacheList(List<Action<EEvent>> toCache)
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
			foreach (KeyValuePair<string, List<Action<EEvent>>> item in eventMap)
			{
				CacheList(item.Value);
			}
			eventMap.Clear();
		}
    }
}

public interface IEventDispatcher
{
	void Add(string type, Action<EEvent> listener);
	void Dispatch(EEvent evt);
	void Remove(string type, Action<EEvent> listener);
	bool Has(string type);
}