using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Firebase.RemoteConfig;
using Newtonsoft.Json;
using Quality.Core.Logger;

// ReSharper disable All

namespace Quality.Core.RemoteConfig
{
    public class FirebaseProvider
    {
        public async UniTask FetchDataAsync()
        {
            await FirebaseRemoteConfig.DefaultInstance.FetchAsync(TimeSpan.Zero);
            await FirebaseRemoteConfig.DefaultInstance.ActivateAsync();
            
            this.Log($"FetchDataAsync Success {FirebaseRemoteConfig.DefaultInstance.AllValues.Count}");
        }
        
        public void OverwriteGroupData(string key, RemoteGroupData groupData)
        {
            var json = FirebaseRemoteConfig.DefaultInstance.GetValue(key).StringValue;

            if (string.IsNullOrEmpty(json))
            {
                this.LogWarning($"Key '{key}' not found or empty in Firebase Remote. Using existing local value.");
                return;
            }

            try
            {
                JsonConvert.PopulateObject(json, groupData);
            }
            catch (JsonException ex)
            {
                this.LogError($"Failed to deserialize key '{key}'. Exception: {ex}. Using existing local value.");
            }
        }

        public void OverwritePrimitiveData(IReadOnlyList<string> keysToFetch, RemotePrimitiveData data)
        {
            var configDictionary = GetPrimitiveDataAsDictionary(keysToFetch);
            
            var json = string.Empty;
            
            try
            {
                json = JsonConvert.SerializeObject(configDictionary, Formatting.Indented);
            }
            catch (JsonException ex)
            {
                this.LogError($"Serializing PrimitiveData to JSON: {ex.Message}");
                return;
            }

            try
            {
                JsonConvert.PopulateObject(json, data);
            }
            catch (JsonException ex)
            {
                this.LogError($"Deserializing JSON to RemoteConfigData: {ex.Message}");
            }
        }

        private Dictionary<string, object> GetPrimitiveDataAsDictionary(IReadOnlyList<string> keysToFetch)
        {
            var configDictionary = new Dictionary<string, object>();

            foreach (var key in keysToFetch)
            {
                ConfigValue value = FirebaseRemoteConfig.DefaultInstance.GetValue(key);
                configDictionary[key] = GetConfigValueAsObject(value);
            }

            return configDictionary;
        }

        private object GetConfigValueAsObject(ConfigValue configValue)
        {
            try
            {
                return configValue.BooleanValue;
            }
            catch
            {
                // ignored
            }

            try
            {
                return configValue.LongValue;
            }
            catch
            {
                // ignored
            }

            try
            {
                return configValue.DoubleValue;
            }
            catch
            {
                // ignored
            }

            return configValue.StringValue;
        }
    }
}
