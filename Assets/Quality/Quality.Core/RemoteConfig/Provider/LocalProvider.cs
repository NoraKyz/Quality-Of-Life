using System.Collections.Generic;
using Newtonsoft.Json;
using Quality.Core.Logger;
using UnityEngine;

namespace Quality.Core.RemoteConfig
{
    public class LocalProvider
    {
        private const string PRIMITIVE_CONFIG_KEY = "primitive_remote_config";
        
        public void Load(IReadOnlyDictionary<string, RemoteGroupData> groupDataMap)
        {
            foreach (var pair in groupDataMap)
            {
                var json = PlayerPrefs.GetString(key, string.Empty);

                if (string.IsNullOrEmpty(json))
                {
                    this.LogWarning($"Key '{key}' not found or empty in PlayerPrefs. Using existing local value.");
                    continue;
                }

                try
                {
                    JsonConvert.PopulateObject(json, data);
                }
                catch (JsonException ex)
                {
                    this.LogError($"Failed to deserialize key '{key}'. Exception: {ex}. Using existing local value.");
                }
            }
        }

        public void Load(RemotePrimitiveData data)
        {
            var json = PlayerPrefs.GetString(PRIMITIVE_CONFIG_KEY, string.Empty);

            if (string.IsNullOrEmpty(json))
            {
                Save(data);
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

        public void Save(RemotePrimitiveData data)
        {
            var json = JsonConvert.SerializeObject(data);
            PlayerPrefs.SetString(CONFIG_KEY, json);
            PlayerPrefs.Save();
        }
        
        public void Save(string key, RemoteGroupData dataToSave)
        {
            var json = JsonConvert.SerializeObject(dataToSave);
            PlayerPrefs.SetString(key, json);
            PlayerPrefs.Save();
        }
    }
}
