using System.Collections.Generic;
using UnityEngine;

namespace Quality.Core.RemoteConfig
{
    public class RemoteConfigKey : ScriptableObject
    {
        [SerializeField] private List<string> _configKey = new();

        public IReadOnlyList<string> ConfigKeys => _configKey;

        internal void ReloadKey()
        {
            _configKey.Clear();

            var keys = RemoteConfigHelper.GetAllConfigKey();

            _configKey.AddRange(keys);

#if UNITY_EDITOR
            UnityEditor.EditorUtility.SetDirty(this);
            UnityEditor.AssetDatabase.SaveAssets();
#endif
        }
    }

#if UNITY_EDITOR
    [UnityEditor.CustomEditor(typeof(RemoteConfigKey))]
    public class RemoteConfigKeySOEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            var remoteConfigKeySO = (RemoteConfigKey)target;

            if (GUILayout.Button("Reload Key"))
            {
                remoteConfigKeySO.ReloadKey();
            }
        }
    }
#endif
}
