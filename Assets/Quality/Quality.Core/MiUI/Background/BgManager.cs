using System.Collections.Generic;
using Alchemy.Inspector;
using UnityEditor;
using UnityEngine;

namespace Quality.Core.MiUI
{
    public class BgManager : MonoBehaviour
    {
        [SerializeField] private BgType   _currentBgType = BgType.NONE;
        [SerializeField] private List<Bg> _bgList        = new();

#if UNITY_EDITOR
        
        [Button]
        private void Reload()
        {
            _bgList = new List<Bg>(GetComponentsInChildren<Bg>(true));
            EditorUtility.SetDirty(this);
        }
        
#endif
        
        public void SetBg(BgType bgType)
        {
            if (_currentBgType == bgType)
            {
                return;
            }

            foreach (var bg in _bgList)
            {
                bg.SetActive(bg.BgType == bgType);
            }

            _currentBgType = bgType;
        }
    }
}
