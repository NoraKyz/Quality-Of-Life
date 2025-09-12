using System;
using System.Collections.Generic;
using CodeStage.AntiCheat.ObscuredTypes.Converters;
using Newtonsoft.Json;
using Quality.Core.Logger;
using Quality.Core.Utilities;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Quality.Core.SaveLoadData
{
    public static class DataManager
    {
        private const string USER_DATA_PATH = "UserData";
        
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
                new JsonSerializerSettings {
                    Converters       = { new ObscuredTypesNewtonsoftConverter() },
                    ContractResolver = new IgnoreSOPropertiesResolver()
                };
        }

        private static void Initialize()
        {
            var userDataAssets = Resources.LoadAll<UserDataBase>(USER_DATA_PATH);

            foreach (var asset in userDataAssets)
            {
                if (s_userData.ContainsKey(asset.GetType()))
                {
                    MyLogger.LogWarning($"[DataManager] Duplicate UserData type: {asset.GetType()}");
                    continue;
                }

                // Trong Editor, tạo một bản sao để tránh thay đổi file asset gốc.
                // Trong Build, sử dụng trực tiếp asset đã load.
#if UNITY_EDITOR
                var runtimeData = Object.Instantiate(asset);
#else
                var runtimeData = asset;
#endif

                s_userData.Add(runtimeData.GetType(), runtimeData);

                if (s_localProvider.Overwrite(runtimeData) == false)
                {
                    s_localProvider.Save(runtimeData);
                }
            }
        }

        public static T Get<T>() where T : UserDataBase
        {
            if (s_userData.TryGetValue(typeof(T), out UserDataBase data))
            {
                return data as T;
            }

            MyLogger.LogWarning($"[DataManager] Not found data with type: {typeof(T)}");
            return null;
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
