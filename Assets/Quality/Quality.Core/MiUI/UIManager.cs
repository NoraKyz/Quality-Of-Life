using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Quality.Core.CustomAttribute;
using Quality.Core.Pattern;
using Quality.Core.Utilities;
using UnityEngine;

namespace Quality.Core.MiUI
{
    public class UIManager : MonoSingleton<UIManager>
    {
        [SerializeField, SelfFill] private Transform    _containerTF;
        [SerializeField]           private UIAssetRefSO _uiAssetRef;
        [SerializeField]           private Canvas       _blockRaycast;
        [SerializeField]           private BgManager    _bgManager;

        private readonly Dictionary<Type, UICanvas>  _uiCanvas        = new();
        private readonly Dictionary<Type, UIRefData> _uiRegistersData = new();

        private readonly List<UniTask<UIPopup>>  _popupRequestQueue  = new();
        private readonly List<UniTask<UIScreen>> _screenRequestQueue = new();

        private ProcessingTracker _processingTracker;

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}
