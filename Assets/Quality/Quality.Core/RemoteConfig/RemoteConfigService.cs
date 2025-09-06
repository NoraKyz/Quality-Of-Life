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
        [SerializeField] private RemoteConfigKey _remoteConfigKey;
        [SerializeField] private RemoteConfigData _remoteConfigData;


        private LocalProvider _localProvider;
        private FirebaseProvider _firebaseProvider;

        private CancellationTokenSource _cst;

        public IReadOnlyRemoteConfigData Config => _remoteConfigData;

        private void Reset()
        {
            _remoteConfigKey = Resources.Load<RemoteConfigKey>("so-remote-config-key");
            _remoteConfigData = Resources.Load<RemoteConfigData>("so-remote-config-data");
        }

        public async UniTask InitializeAsync()
        {
            _localProvider = new LocalProvider();
            _firebaseProvider = new FirebaseProvider();

            _localProvider.Load(_remoteConfigData);

            FetchFirebaseConfig().Forget();

            _cst = new CancellationTokenSource();
            await UniTask.Delay(TimeSpan.FromSeconds(3f), cancellationToken: _cst.Token).SuppressCancellationThrow();
        }

        private async UniTaskVoid FetchFirebaseConfig()
        {
            try
            {
                await _firebaseProvider.FetchDataAsync(_remoteConfigKey.ConfigKeys, _remoteConfigData);

                _localProvider.Save(_remoteConfigData);
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
