using UnityEngine;

namespace Core.Modules.Architecture.RefLocator
{
    public abstract class RefBase : MonoBehaviour
    {
        protected virtual void OnDestroy()
        {
            Refs.Remove(this);
        }
    }
}
