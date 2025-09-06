using Newtonsoft.Json;
using Quality.Core.Logger;
using UnityEngine;

namespace Quality.Core.RemoteConfig
{
    public class LocalProvider
    {
        private const string CONFIG_KEY = "remote_config";

        public void Load(RemoteConfigData data)
        {
            var json = PlayerPrefs.GetString(CONFIG_KEY, string.Empty);

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

        public void Save(RemoteConfigData data)
        {
            var json = JsonConvert.SerializeObject(data);
            PlayerPrefs.SetString(CONFIG_KEY, json);
            PlayerPrefs.Save();
        }
    }
}
