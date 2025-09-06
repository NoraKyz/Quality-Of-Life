using UnityEngine;

namespace Quality.Core.Pattern
{
    public class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T s_instance;

        public static T Ins
        {
            get
            {
                if (s_instance == null)
                {
                    // Find singleton
                    s_instance = FindFirstObjectByType<T>();

                    // Create new instance if one doesn't already exist.
                    if (s_instance == null)
                    {
                        var singletonObject = new GameObject();
                        s_instance           = singletonObject.AddComponent<T>();
                        singletonObject.name = typeof(T) + " - Singleton_AutoCreated";
                    }
                }

                return s_instance;
            }
        }

        protected virtual void OnDestroy()
        {
            s_instance = null;
        }
    }
}
