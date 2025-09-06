using UnityEngine;

namespace Quality.Core.Cameraman
{
    public class CameraController : MonoBehaviour
    {
        private static Camera s_mainCamera;

        public static Camera MainCamera
        {
            get
            {
                if (s_mainCamera == null)
                {
                    s_mainCamera = Camera.main;
                }

                return s_mainCamera;
            }
        }
        
        [SerializeField] private SpriteRenderer _targetView;

        private void Awake()
        {
            s_mainCamera = Camera.main;
        }

        private void Start()
        {
            if(_targetView == null) return;

            SetOrthographicSize(_targetView.bounds.size.x, _targetView.bounds.size.y);
        }

        public void SetOrthographicSize(float targetX, float targetY)
        {
            var screenRatio = 1f * Screen.width / Screen.height;
            var targetRatio = targetX / targetY;

            if (screenRatio >= targetRatio)
            {
                s_mainCamera.orthographicSize = targetY / 2;
            }
            else
            {
                var differenceInSize = targetRatio / screenRatio;
                s_mainCamera.orthographicSize = targetY / 2 * differenceInSize;
            }
        }

        private void OnDestroy()
        {
            s_mainCamera = null;
        }
    }
}
