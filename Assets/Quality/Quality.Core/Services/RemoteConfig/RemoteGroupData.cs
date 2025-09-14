using Newtonsoft.Json;
using Quality.Core.Logger;
using UnityEngine;
using VInspector;

namespace Quality.Core.RemoteConfig
{
    [JsonObject(MemberSerialization.OptIn)]
    public abstract class RemoteGroupData : ScriptableObject
    {
        [Button]
        public void LogData()
        {
            this.LogWarning(JsonConvert.SerializeObject(this));
        }
    }
}
