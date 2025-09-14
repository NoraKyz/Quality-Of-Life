using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Quality.Core.MiUI
{
    public sealed class UIPopupProvider
    {
        private Transform _uiRoot;

        private List<UIPopup> _activePopups = new();
        private Dictionary<int, UIPopup> _createdPopups = new();
        private List<int> _popupRequests = new();

        public async UniTask<T> Show<T>(AssetReference popupRef) where T : UIPopup
        {
            var id = typeof(T).MetadataToken;

            if (_activePopups.Count == 0 && _popupRequests.Count == 0)
            {
                return await ShowImmediately<T>(popupRef);
            }
            else
            {
                _popupRequests.Add(id);

            }
        }

        public async UniTask<T> ShowImmediately<T>(AssetReference popupRef) where T : UIPopup
        {
            var id = typeof(T).MetadataToken;

            T popup = await CreatePopup<T>(popupRef);
            popup.gameObject.SetActive(true);
            popup.Open();

            _activePopups.Insert(0, popup);
            _popupRequests.RemoveAll(x => x == id);

            return popup;
        }

        private async UniTask<T> CreatePopup<T>(AssetReference popupRef) where T : UIPopup
        {
            var id = typeof(T).MetadataToken;

            if (_createdPopups.TryGetValue(id, out var existingPopup))
            {
                return (T)existingPopup;
            }

            var popup = await Addressables.InstantiateAsync(popupRef, _uiRoot);
            popup.SetActive(false);

            var uiPopup = popup.GetComponent<T>();
            uiPopup.Setup();

            _createdPopups[id] = uiPopup;
            return uiPopup;
        }
    }
}
