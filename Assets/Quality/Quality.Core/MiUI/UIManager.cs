// using System;
// using System.Collections.Generic;
// using Cysharp.Threading.Tasks;
// using Quality.Core.CustomAttribute;
// using Quality.Core.Logger;
// using Quality.Core.Pattern;
// using UnityEngine;
// using UnityEngine.AddressableAssets;
//
// namespace Quality.Core.MiUI
// {
//     public class UIManager : MonoSingleton<UIManager>
//     {
//         [SerializeField, SelfFill] private Transform    _containerTF;
//         [SerializeField]           private UIAssetRefSO _uiAssetRef;
//         [SerializeField]           private Canvas       _blockRaycast;
//         [SerializeField]           private BgManager    _bgManager;
//
//         private readonly Dictionary<Type, UICanvas>  _uiCanvasInstances = new();
//         private readonly Dictionary<Type, UIRefData> _uiRegisterData    = new();
//         private readonly List<UIPopup>               _activePopups      = new();
//
//         private readonly Queue<Func<UniTask>> _screenRequestQueue = new();
//         private readonly Queue<Func<UniTask>> _popupRequestQueue  = new();
//
//         private UIScreen _currentScreen;
//         private bool     _isScreenProcessing;
//         private bool     _isPopupProcessing;
//
//         protected void Awake()
//         {
//             DontDestroyOnLoad(gameObject);
//             InitializeUIRegister();
//         }
//
//         private void InitializeUIRegister()
//         {
//             foreach (var refData in _uiAssetRef.Refs)
//             {
//                 var type = Type.GetType(refData.NameType);
//
//                 if (type != null)
//                 {
//                     _uiRegisterData[type] = refData;
//                 }
//                 else
//                 {
//                     this.LogError($"Cannot find Type with name: {refData.NameType}");
//                 }
//             }
//         }
//
//         public bool IsOpened<T>() where T : UICanvas
//         {
//             var canvas = _uiCanvasInstances.GetValueOrDefault(typeof(T), null);
//             return canvas != null && canvas.gameObject.activeSelf;
//         }
//
//         #region Screen Management
//
//         public async UniTask<T> ShowScreen<T>() where T : UIScreen
//         {
//             var tcs = new UniTaskCompletionSource<T>();
//
//             _screenRequestQueue.Enqueue(async () =>
//             {
//                 var screen = await GetOrCreateUI<T>();
//                 tcs.TrySetResult(screen);
//                 await ProcessShowScreen(screen);
//             });
//
//             ProcessScreenQueue();
//
//             return await tcs.Task;
//         }
//
//         private async UniTask ProcessShowScreen<T>(T screen) where T : UIScreen
//         {
//             if (_currentScreen != null)
//             {
//                 await _currentScreen.Close();
//             }
//
//             _currentScreen = screen;
//             _bgManager.SetBg(screen.BgType);
//
//             _blockRaycast.enabled = true;
//             await screen.Open();
//             _blockRaycast.enabled = false;
//         }
//
//         public void CloseScreen<T>() where T : UIScreen
//         {
//             if (_currentScreen != null && _currentScreen.GetType() == typeof(T))
//             {
//                 _screenRequestQueue.Enqueue(async () =>
//                 {
//                      await _currentScreen.Close();
//                     _currentScreen = null;
//                 });
//                 ProcessScreenQueue();
//             }
//         }
//
//         private async void ProcessScreenQueue()
//         {
//             if (_isScreenProcessing || _screenRequestQueue.Count == 0)
//             {
//                 return;
//             }
//
//             _isScreenProcessing = true;
//
//             while (_screenRequestQueue.Count > 0)
//             {
//                 var request = _screenRequestQueue.Dequeue();
//                 await request();
//             }
//
//             _isScreenProcessing = false;
//         }
//
//         #endregion
//
//         #region Popup Management
//
//         public async UniTask<T> ShowPopup<T>(bool waitForQueue = false) where T : UIPopup
//         {
//             if (!waitForQueue)
//             {
//                 // Hiển thị đè, không cần chờ đợi
//                 var popup = await GetOrCreateUI<T>();
//                 await ProcessShowPopup(popup);
//                 return popup;
//             }
//
//             // Hiển thị theo hàng đợi
//             var tcs = new UniTaskCompletionSource<T>();
//
//             _popupRequestQueue.Enqueue(async () =>
//             {
//                 var popup = await GetOrCreateUI<T>();
//                 tcs.TrySetResult(popup);
//                 await ProcessShowPopup(popup);
//             });
//
//             ProcessPopupQueue();
//
//             return await tcs.Task;
//         }
//
//         private async UniTask ProcessShowPopup<T>(T popup) where T : UIPopup
//         {
//             _activePopups.Add(popup);
//             _bgManager.SetBg(popup.BgType);
//
//             _blockRaycast.enabled = true;
//             await popup.Open();
//             _blockRaycast.enabled = false;
//         }
//
//         public async void ClosePopup<T>() where T : UIPopup
//         {
//             var popup = _uiCanvasInstances.GetValueOrDefault(typeof(T), null) as T;
//             if (popup != null && _activePopups.Contains(popup))
//             {
//                 _blockRaycast.enabled = true;
//                 await popup.Close();
//                 _activePopups.Remove(popup);
//                 _blockRaycast.enabled = false;
//
//                 // Nếu popup này thuộc hàng đợi, xử lý mục tiếp theo
//                 if (_isPopupProcessing)
//                 {
//                     ProcessPopupQueue();
//                 }
//             }
//         }
//
//         private async void ProcessPopupQueue()
//         {
//             if (_isPopupProcessing || _popupRequestQueue.Count == 0)
//             {
//                 return;
//             }
//
//             _isPopupProcessing = true;
//             while (_popupRequestQueue.Count > 0)
//             {
//                 var request = _popupRequestQueue.Dequeue();
//                 await request();
//             }
//             _isPopupProcessing = false;
//         }
//
//         #endregion
//
//         private async UniTask<T> GetOrCreateUI<T>() where T : UICanvas
//         {
//             if (_uiCanvasInstances.TryGetValue(typeof(T), out var canvas) && canvas != null)
//             {
//                 return canvas as T;
//             }
//
//             var refData = _uiRegisterData[typeof(T)];
//             var go = await Addressables.InstantiateAsync(refData.AssetReference, _containerTF);
//             go.SetActive(false); // Disable trước khi init và setup
//
//             var newCanvas = go.GetComponent<T>();
//             newCanvas.Init();
//             _uiCanvasInstances[typeof(T)] = newCanvas;
//
//             return newCanvas;
//         }
//     }
// }
