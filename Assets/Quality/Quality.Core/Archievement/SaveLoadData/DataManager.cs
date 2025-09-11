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
        private static Dictionary<Type, UserDataBase> s_userData = new();

#if UNITY_EDITOR

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void Init()
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
            Type type = typeof(T);

            if (s_userData.TryGetValue(type, out UserDataBase value))
            {
                return value as T;
            }

            var rawData = PlayerPrefs.GetString(type.Name, string.Empty);
            T   data;

            if (rawData != string.Empty)
            {
                data = JsonConvert.DeserializeObject<T>(rawData);
                data.InitPresentData();
                s_userData[type] = data;
            }
            else
            {
                data             = new T();
                s_userData[type] = data;
                Save<T>();
            }

            return data;
        }

        public static void Save<T>() where T : UserDataBase
        {
            Type type = typeof(T);

            if (!s_userData.TryGetValue(type, out UserDataBase value))
            {
                MyLogger.LogWarning($"[DataManager] No user data found for type {type}");
                return;
            }

            var data = JsonConvert.SerializeObject(value);

            PlayerPrefs.SetString(value.DataKey, data);
        }
    }
}
