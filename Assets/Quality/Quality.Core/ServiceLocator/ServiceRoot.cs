using UnityEngine;

namespace Quality.Core.ServiceLocator
{
    [DefaultExecutionOrder(-100)]
    public class ServiceRoot : MonoBehaviour
    {
        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
            
            var services = GetComponentsInChildren<ServiceBase>(true);
            Services.Preload(services);
        }
    }
}
