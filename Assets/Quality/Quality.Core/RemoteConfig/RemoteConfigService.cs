using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Quality.Core.Logger;
using Quality.Core.ServiceLocator;
using UnityEngine;

namespace Quality.Core.RemoteConfig
{
    public class RemoteConfigService : ServiceBase
    {
        [SerializeField] private RemotePrimitiveData      _remotePrimitiveData;
        [SerializeField] private RemotePrimitiveKeySO     _remotePrimitiveKeySO;
        [SerializeField] private RemoteGroupDefaultDataSO _remoteGroupDefaultDataSO;

        private LocalProvider    _localProvider;
        private FirebaseProvider _firebaseProvider;

        private CancellationTokenSource _cst;

        private RemotePrimitiveData _remotePrimitiveData;

        private void Reset()
        {
            _remotePrimitiveKeySO = Resources.Load<RemotePrimitiveKeySO>("so-remote-config-key");
            _remotePrimitiveData  = Resources.Load<RemotePrimitiveData>("so-remote-config-data");
        }

        public async UniTask InitializeAsync()
        {
            _localProvider    = new LocalProvider();
            _firebaseProvider = new FirebaseProvider();

            _localProvider.Load(_remotePrimitiveData);

            FetchFirebaseConfig().Forget();

            _cst = new CancellationTokenSource();
            await UniTask.Delay(TimeSpan.FromSeconds(3f), cancellationToken: _cst.Token).SuppressCancellationThrow();
        }

        private async UniTaskVoid FetchFirebaseConfig()
        {
            try
            {
                await _firebaseProvider.OverwritePrimitiveData(_remotePrimitiveKeySO.ConfigKeys, _remotePrimitiveData);

                _localProvider.Save(_remotePrimitiveData);
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
