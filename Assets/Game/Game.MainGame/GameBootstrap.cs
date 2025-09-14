using Cysharp.Threading.Tasks;
using Quality.Core.Logger;
using Quality.Core.MiUI;
using Quality.Core.RemoteConfig;
using Quality.Core.ServiceLocator;
using UnityEngine;

namespace Game.MainGame
{
    public class GameBootstrap : MonoBehaviour
    {
        public void Awake()
        {
            Input.multiTouchEnabled     = false;
            Application.targetFrameRate = 60;
            Screen.sleepTimeout         = SleepTimeout.NeverSleep;
        }

        private void Start()
        {
            InitializeAsync().Forget();
        }

        private async UniTask InitializeAsync()
        {
            await ServicesProvider.Get<RemoteConfigService>().InitializeAsync();
        }
    }
}
