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
        private const string CONFIG_KEY = "remote_config";

        public async UniTask FetchDataAsync(IReadOnlyList<string> keysToFetch, RemoteConfigData data)
        {
            await FirebaseRemoteConfig.DefaultInstance.FetchAsync();
            await FirebaseRemoteConfig.DefaultInstance.ActivateAsync();

            var configDictionary = GetValuesAsDictionary(keysToFetch);

            this.Log($"Fetched {configDictionary.Count.ToString()} keys from Firebase.");

            var json = JsonConvert.SerializeObject(configDictionary, Formatting.Indented);

            try
            {
                JsonConvert.PopulateObject(json, data);
            }
            catch (JsonException ex)
            {
                this.LogError($"Deserializing JSON to RemoteConfigData: {ex.Message}");
            }
        }

        private Dictionary<string, object> GetValuesAsDictionary(IReadOnlyList<string> keysToFetch)
        {
            var configDictionary = new Dictionary<string, object>();

            foreach (var key in keysToFetch)
            {
                ConfigValue value = FirebaseRemoteConfig.DefaultInstance.GetValue(key);
                configDictionary[key] = GetValueAsObject(value);
            }

            return configDictionary;
        }

        private object GetValueAsObject(ConfigValue configValue)
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
