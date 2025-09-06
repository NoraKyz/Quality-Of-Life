using System;
using System.Collections.Generic;
using Quality.Core.Logger;
using UnityEngine;

namespace Core.Modules.Architecture.RefLocator
{
    // Cache ref "singleton" instances in the scene.
    public static class Refs
    {
        private static Dictionary<Type, RefBase> s_refs = new();

#if UNITY_EDITOR

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void Init()
        {
            s_refs.Clear();
        }

#endif

        public static void Preload(RefBase[] refBases)
        {
            s_refs.Clear();

            foreach (var refBase in refBases)
            {
                Add(refBase);
            }
        }

        public static T Get<T>() where T : RefBase
        {
            var type = typeof(T);

            if (s_refs.TryGetValue(type, out var refBase))
            {
                return (T)refBase;
            }

            MyLogger.LogError($"Service of type {type} is not registered.");

            return null;
        }

        internal static void Remove<T>(T refBase) where T : RefBase
        {
            Type type = refBase.GetType();
            s_refs.Remove(type);
        }

        private static void Add<T>(T refBase) where T : RefBase
        {
            Type type = refBase.GetType();

            if (s_refs.TryAdd(type, refBase))
            {
                return;
            }

            MyLogger.LogWarning($"[Refs] Has multiple of type {type.Name} to register.");
        }
    }
}
