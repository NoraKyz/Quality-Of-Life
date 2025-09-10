using Newtonsoft.Json;
using Quality.Core.Logger;
using UnityEngine;

namespace Quality.Core.RemoteConfig
{
    public class LocalProvider
    {
        private const string PRIMITIVE_CONFIG_KEY = "primitive_remote_config";

        public void OverwriteGroupData(string key, RemoteGroupData groupData)
        {
            var json = PlayerPrefs.GetString(key, string.Empty);

            if (string.IsNullOrEmpty(json))
            {
                this.LogWarning($"Key '{key}' not found or empty in PlayerPrefs. Using existing default value.");
                return;
            }

            try
            {
                JsonConvert.PopulateObject(json, groupData);
            }
            catch (JsonException ex)
            {
                this.LogError($"Failed to deserialize key '{key}'. Exception: {ex}. Using existing default value.");
            }
        }

        public void OverwritePrimitiveData(RemotePrimitiveData data)
        {
            var json = PlayerPrefs.GetString(PRIMITIVE_CONFIG_KEY, string.Empty);

            if (string.IsNullOrEmpty(json))
            {
                this.LogWarning("Primitive config not found or empty in PlayerPrefs. Using with default value.");
                return;
            }

            try
            {
                JsonConvert.PopulateObject(json, data);
            }
            catch (JsonException ex)
            {
                this.LogError($"Failed to deserialize PrimitiveData. Exception: {ex}. Using existing default value." );
            }
        }

        public void Save(RemotePrimitiveData data)
        {
            var json = JsonConvert.SerializeObject(data);
            PlayerPrefs.SetString(PRIMITIVE_CONFIG_KEY, json);
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
