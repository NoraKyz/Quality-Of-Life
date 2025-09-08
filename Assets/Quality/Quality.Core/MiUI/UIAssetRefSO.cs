using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using VInspector;

namespace Quality.Core.MiUI
{
    [Serializable]
    public class RefData
    {
        [SerializeField, ReadOnly] private string         typeName;
        [SerializeField, ReadOnly] private AssetReference assetReference;

        public string NameType
        {
            get => typeName;
            set => typeName = value;
        }

        public AssetReference AssetReference
        {
            get => assetReference;
            set => assetReference = value;
        }
    }

    public class UIAssetRefSO : ScriptableObject
    {
        [SerializeField] private string        _targetFolder;
        [SerializeField] private List<RefData> _refs = new List<RefData>();

        public IReadOnlyList<RefData> Refs => _refs;

#if UNITY_EDITOR

        [Button]
        private void Reload()
        {
            _refs.Clear();

            var prefabGuids = UnityEditor.AssetDatabase.FindAssets("t:Prefab", new[] { _targetFolder });

            foreach (var guid in prefabGuids)
            {
                var        assetPath = UnityEditor.AssetDatabase.GUIDToAssetPath(guid);
                GameObject prefab    = UnityEditor.AssetDatabase.LoadAssetAtPath<GameObject>(assetPath);

                if (prefab == null) continue;

                var uiPanel = prefab.GetComponent<UICanvas>();

                if (uiPanel == null)
                {
                    continue;
                }

                var typeName = uiPanel.GetType().Name;
                _refs.Add(new RefData { NameType = typeName, AssetReference = new AssetReference(guid) });
            }
        }

#endif
    }
}
