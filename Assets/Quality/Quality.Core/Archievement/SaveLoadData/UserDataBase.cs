using Newtonsoft.Json;
using Quality.Core.Logger;
using UnityEngine;
using VInspector;

namespace Quality.Core.SaveLoadData
{
    [JsonObject(MemberSerialization.Fields)]
    public abstract class UserDataBase : ScriptableObject
    {
        public abstract string Key { get; }

        [Button]
        public void LogData()
        {
            this.LogWarning(JsonConvert.SerializeObject(this));
        }
    }
}
