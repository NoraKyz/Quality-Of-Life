using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Quality.Core.CustomAttribute;
using Quality.Core.Logger;
using Quality.Core.ServiceLocator;
using UnityEngine;

namespace Quality.Core.RemoteConfig
{
    public class RemoteConfigService : ServiceBase
    {
        [SerializeField, SelfFill("so-remote-primitive-data")]
        private RemotePrimitiveData _remotePrimitiveData;

        [SerializeField, SelfFill("so-remote-primitive-key")]
        private RemotePrimitiveKeySO _remotePrimitiveKeySO;

        [SerializeField, SelfFill("so-remote-group-data")]
        private RemoteGroupDefaultDataSO _remoteGroupDefaultDataSO;

        private LocalProvider    _localProvider;
        private FirebaseProvider _firebaseProvider;

        private CancellationTokenSource _cst;

        private Dictionary<Type, RemoteGroupData> _groupDataCache = new();

        public async UniTask InitializeAsync()
        {
            foreach (var defaultData in _remoteGroupDefaultDataSO.Data.Values)
            {
                _groupDataCache.Add(defaultData.GetType(), defaultData);
            }

            _localProvider    = new LocalProvider();
            _firebaseProvider = new FirebaseProvider();

            OverwirteDataWithLocalProvider();

            FetchFirebaseConfig().Forget();

            _cst = new CancellationTokenSource();
            await UniTask.Delay(TimeSpan.FromSeconds(3f), cancellationToken: _cst.Token).SuppressCancellationThrow();
        }

        public RemotePrimitiveData GetPrimitiveData()
        {
            return _remotePrimitiveData;
        }

        public T GetGroupData<T>() where T : RemoteGroupData
        {
            if (_groupDataCache.TryGetValue(typeof(T), out var data))
            {
                return (T)data;
            }

            this.LogError($"Group data of type {typeof(T)} not found.");
            return null;
        }

        private void OverwirteDataWithLocalProvider()
        {
            _localProvider.OverwritePrimitiveData(_remotePrimitiveData);

            foreach (var (key, defaultData) in _remoteGroupDefaultDataSO.Data)
            {
                _localProvider.OverwriteGroupData(key, defaultData);
            }
        }

        private async UniTaskVoid FetchFirebaseConfig()
        {
            try
            {
                await _firebaseProvider.FetchDataAsync();

                _firebaseProvider.OverwritePrimitiveData(_remotePrimitiveKeySO.ConfigKeys, _remotePrimitiveData);
                _localProvider.Save(_remotePrimitiveData);

                foreach (var (key, defaultData) in _remoteGroupDefaultDataSO.Data)
                {
                    _firebaseProvider.OverwriteGroupData(key, defaultData);
                    _localProvider.Save(key, defaultData);
                }

                this.Log("Fetch and save remote config success.");
            }
            catch (Exception e)
            {
                this.LogError($"Fetch remote config failed: {e.Message}");
            }
            finally
            {
                _cst?.Cancel();
                _cst?.Dispose();
                _cst = null;
            }
        }
    }
}
