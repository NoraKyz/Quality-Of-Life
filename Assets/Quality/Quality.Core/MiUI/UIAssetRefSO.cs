using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using VInspector;

namespace Quality.Core.MiUI
{
    [Serializable]
    public class UIRefData
    {
        [SerializeField]           private bool           _isPreload;
        [SerializeField, ReadOnly] private string         _typeName;
        [SerializeField, ReadOnly] private AssetReference _assetReference;

        public bool IsPreload
        {
            get => _isPreload;
            internal set => _isPreload = value;
        }

        public string NameType
        {
            get => _typeName;
            internal set => _typeName = value;
        }

        public AssetReference AssetReference
        {
            get => _assetReference;
            internal set => _assetReference = value;
        }
    }

    public class UIAssetRefSO : ScriptableObject
    {
        [SerializeField] private string          _targetFolder;
        [SerializeField] private List<UIRefData> _refs = new List<UIRefData>();

        public IReadOnlyList<UIRefData> Refs => _refs;

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
                _refs.Add(new UIRefData { NameType = typeName, AssetReference = new AssetReference(guid) });
            }
        }

#endif
    }
}
