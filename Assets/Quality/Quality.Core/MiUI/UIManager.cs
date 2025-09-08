using Quality.Core.Pattern;

namespace Quality.Core.MiUI
{
    public class UIManager : MonoSingleton<UIManager>
    {
        
        
        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}
