using System.Collections.Generic;
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
    
    public sealed class RemoteGroupDefaultDataSO : ScriptableObject
    {
        [SerializeField] private SerializedDictionary<string, RemoteGroupData> _data = new();
        
        public IReadOnlyDictionary<string, RemoteGroupData> Data => _data;
    }
}
