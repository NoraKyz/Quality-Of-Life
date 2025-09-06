using UnityEngine;

namespace Core.Modules.Architecture.RefLocator
{
    [DefaultExecutionOrder(-50)]
    public class RefRoot : MonoBehaviour
    {
        private void Awake()
        {
            var refs = GetComponentsInChildren<RefBase>(true);
            Refs.Preload(refs);
        }
    }
}
