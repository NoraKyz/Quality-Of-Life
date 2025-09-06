using System.Collections.Generic;
using UnityEngine;

namespace Quality.Core.FastComponent
{
    internal abstract class CacheBase
    {
        public abstract void Clear();
    }
    
    internal class Cache<T> : CacheBase where T : Component
    {
        private Dictionary<int, T> _dict = new ();

        public T Get(GameObject gameObject)
        {
            if (_dict.TryGetValue(gameObject.GetInstanceID(), out var value))
            {
                return value;
            }

            T component = gameObject.GetComponent<T>();

            if (component != null)
            {
                _dict[gameObject.GetInstanceID()] = component;
            }

            return component;
        }

        public override void Clear()
        {
            _dict.Clear();
        }
    }
}
