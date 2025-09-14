using System;
using System.Collections.Generic;
using CodeStage.AntiCheat.ObscuredTypes.Converters;
using Newtonsoft.Json;
using Quality.Core.Logger;
using UnityEngine;

namespace Quality.Core.SaveLoadData
{
    public static class DataManager
    {
        private static readonly Dictionary<Type, UserDataBase> s_userData      = new();
        private static readonly LocalProvider                  s_localProvider = new();

#if UNITY_EDITOR
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void ResetOnDontLoadDomain()
        {
            s_userData.Clear();
        }
#endif

        static DataManager()
        {
            JsonConvert.DefaultSettings = () =>
                new JsonSerializerSettings { Converters = { new ObscuredTypesNewtonsoftConverter() } };
        }

        public static T Get<T>() where T : UserDataBase, new()
        {
            if (s_userData.TryGetValue(typeof(T), out UserDataBase data))
            {
                return data as T;
            }

            data = new T();
            s_userData.Add(typeof(T), data);

            if (s_localProvider.Overwrite(data))
            {
                s_localProvider.Save(data);
            }

            return data as T;
        }

        public static void Save<T>() where T : UserDataBase
        {
            if (s_userData.TryGetValue(typeof(T), out UserDataBase data))
            {
                s_localProvider.Save(data);
            }
            else
            {
                MyLogger.LogWarning($"[DataManager] Can't save because not found data with type: {typeof(T)}");
            }
        }
    }
}
