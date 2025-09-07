using UnityEngine;

namespace Quality.Core.MiUI
{
    public class Bg : MonoBehaviour
    {
        [SerializeField] private BgType _bgType;

        public BgType BgType => _bgType;

        public void SetActive(bool active)
        {
            gameObject.SetActive(active);
        }
    }
}
