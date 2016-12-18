using UnityEngine;
using max.events;
using System.Collections;
using System.Collections.Generic;

namespace max.events
{
    public class EEvent
    {
        private static List<EEvent> cache = new List<EEvent>();

        public EventDispatcher target{ get{ return _target;} }
        public EventDispatcher currentTarget{ get{ return _currentTarget;} }
        public string type{ get{ return _type;} }
        public bool bubbles{ get{ return _bubbles;} }
        public object data{ get{ return _data;} }
        public bool stopPropagation{ get{ return _stopPropagation;} }
        public bool stopImmediatePropagation{ get{ return _stopImmediatePropagation;} }

        EventDispatcher _target;
        EventDispatcher _currentTarget;
        string _type;
        bool _bubbles;
        object _data;
        bool _stopPropagation = false;
        bool _stopImmediatePropagation = false;

        public EEvent(string type, bool bubbles = false, object data = null)
        {
            this._type = type;
            this._bubbles = bubbles;
            this._data = data;
        }

        public void SetCurrentTarget(EventDispatcher current)
        {
            _currentTarget = current;
        }

        public void SetTarget(EventDispatcher target)
        {
            _target = target;
        }

        internal static EEvent FromPool(string type, bool bubbles = false, object data = null)
        {
            EEvent evt;
            if (cache.Count > 0)
            {
                evt = cache[0];
                evt.ResetEvent(type, bubbles, data);
                cache.RemoveAt(0);
            }
            else
            {
                evt = new EEvent(type, bubbles, data);
            }
            return evt;
        }

        internal static void ToPool(EEvent evt)
        {
            evt._data = evt._target = evt._currentTarget = null;
            cache.Add(evt);
        }

        private void ResetEvent(string type, bool bubbles = false, object data = null)
        {
            this._type = type;
            this._bubbles = bubbles;
            this._data = data;
            _target = _currentTarget = null;
            _stopPropagation = _stopImmediatePropagation = false;
        }
    }
}