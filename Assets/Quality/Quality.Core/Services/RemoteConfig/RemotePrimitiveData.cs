using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using VInspector;

namespace Quality.Core.RemoteConfig
{
    [JsonObject(MemberSerialization.OptIn)]
    public class RemotePrimitiveData : ScriptableObject
    {
        // define your remote config data structure here

        private void Awake()
        {
            hideFlags = HideFlags.DontUnloadUnusedAsset;
        }
    }
    
    public class RemotePrimitiveKeySO : ScriptableObject
    {
        [SerializeField] private List<string> _configKey = new();

        public IReadOnlyList<string> ConfigKeys => _configKey;

        [Button]
        internal void ReloadKey()
        {
            _configKey.Clear();

            var keys = RemotePrimitiveDataHelpers.GetAllConfigKey();

            _configKey.AddRange(keys);

#if UNITY_EDITOR
            UnityEditor.EditorUtility.SetDirty(this);
            UnityEditor.AssetDatabase.SaveAssets();
#endif
        }
    }
}
