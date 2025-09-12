using System;
using System.Collections.Generic;
using Quality.Core.Logger;
using UnityEngine;

namespace Quality.Core.ServiceLocator
{
    public static class ServicesProvider
    {
        private static Dictionary<Type, ServiceBase> s_services = new();

#if UNITY_EDITOR

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void ResetOnDontLoadDomain()
        {
            s_services.Clear();
        }

#endif

        public static T Get<T>() where T : ServiceBase
        {
            var type = typeof(T);

            if (s_services.TryGetValue(type, out var service))
            {
                return (T)service;
            }
            
            MyLogger.LogError($"Service of type {type} is not registered.");

            return null;
        }
        
        internal static void Preload(ServiceBase[] services)
        {
            foreach (var service in services)
            {
                Register(service);
            }
        }

        private static void Register<T>(T service) where T : ServiceBase
        {
            var type = service.GetType();

            if (s_services.TryAdd(type, service))
            {
                return;
            }
            
            MyLogger.LogWarning($"[ServiceLocator] Has multiple of type {type.Name} to register.");
        }
    }
}
