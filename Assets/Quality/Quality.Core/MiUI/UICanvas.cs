using PrimeTween;
using Quality.Core.CustomAttribute;
using UnityEngine;
using VInspector;

namespace Quality.Core.MiUI
{
    [RequireComponent(typeof(Canvas), typeof(CanvasGroup))]
    public abstract class UICanvas : MonoBehaviour
    {
        private const string UI_LAYER_NAME = "UI";

        [Tab("References")]
        [SerializeField, SelfFill] protected Canvas      canvas;
        [SerializeField, SelfFill] protected CanvasGroup canvasGroup;

        [Tab("Settings")]
        [SerializeField] protected int   _sortingOrder      = 0;
        [SerializeField] protected bool  _useDefaultOpening = true;
        [SerializeField] protected float _openLockDuration;

        [SerializeField, ShowIf("_openLockDuration")]
        protected float _openDuration;

        [SerializeField] protected bool _useDefaultClosing = true;

        [SerializeField, ShowIf("_useDefaultClosing")]
        protected float _closeDuration;

        [SerializeField] protected float _closeLockDuration;

        public virtual void Open()
        {
            transform.SetAsLastSibling();
            gameObject.SetActive(true);

            canvas.overrideSorting  = true;
            canvas.sortingLayerName = UI_LAYER_NAME;
            canvas.sortingOrder     = _sortingOrder;

            if (_useDefaultClosing)
            {
                canvasGroup.alpha = 0f;
                Tween.Alpha(canvasGroup, 1f, _openDuration, Ease.OutCubic);
            }
        }

        public virtual void Close()
        {
            if (_useDefaultClosing)
            {
                Tween.Alpha(canvasGroup, 0f, _closeDuration, Ease.InCubic);
            }
        }
        
        public virtual void CloseDirectly()
        {
            gameObject.SetActive(false);
        }
    }
}
