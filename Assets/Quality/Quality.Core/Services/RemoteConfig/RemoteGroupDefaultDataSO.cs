using System.Collections.Generic;
using UnityEngine;
using VInspector;

namespace Quality.Core.RemoteConfig
{
    public sealed class RemoteGroupDefaultDataSO : ScriptableObject
    {
        [SerializeField] private SerializedDictionary<string, RemoteGroupData> _data = new();
        
        public IReadOnlyDictionary<string, RemoteGroupData> Data => _data;
    }
}
