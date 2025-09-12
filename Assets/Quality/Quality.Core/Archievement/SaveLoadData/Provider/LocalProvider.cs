using System;
using Newtonsoft.Json;
using Quality.Core.Logger;
using UnityEngine;

namespace Quality.Core.SaveLoadData
{
    internal class LocalProvider
    {
        public bool Overwrite<T>(T userData) where T : UserDataBase
        {
            var result  = true;
            var rawData = PlayerPrefs.GetString(userData.Key, string.Empty);

            if (rawData != string.Empty)
            {
                try
                {
                    JsonConvert.PopulateObject(rawData, userData);
                }
                catch (Exception e)
                {
                    result = false;
                    this.LogError($"Fail to overwrite user data. Key: {userData.Key}\n{e.Message}");
                }
            }
            else
            {
                result = false;
            }

            return result;
        }

        public void Save<T>(T userData) where T : UserDataBase
        {
            var rawData = JsonConvert.SerializeObject(userData);
            PlayerPrefs.SetString(userData.Key, rawData);
            PlayerPrefs.Save();
        }
    }
}
