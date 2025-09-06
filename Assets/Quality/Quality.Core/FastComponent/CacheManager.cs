using System;
using System.Collections.Generic;
using UnityEngine;

namespace Quality.Core.FastComponent
{
    public class CacheManager
    {
        private static Dictionary<Type, CacheBase> s_dictCache = new();

#if UNITY_EDITOR

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void CleanWhenDisableLoadDomain()
        {
            ClearAll();
        }

#endif
        
        public static T Get<T>(GameObject gameObject) where T : Component
        {
            var type = typeof(T);

            if (!s_dictCache.TryGetValue(type, out var cache))
            {
                var newCache = new Cache<T>();
                s_dictCache[type] = newCache;
                return newCache.Get(gameObject);
            }

            return ((Cache<T>)cache).Get(gameObject);
        }

        public static void Clear<T>() where T : Component
        {
            var type = typeof(T);

            if (s_dictCache.TryGetValue(type, out var cache))
            {
                cache.Clear();
            }
        }

        public static void ClearAll()
        {
            foreach (var cache in s_dictCache.Values)
            {
                cache.Clear();
            }

            s_dictCache.Clear();
        }
    }
}
