using Quality.Core.CustomAttribute;
using UnityEngine;
using UnityEngine.Playables;
using VInspector;

namespace Quality.Core.MiUI
{
    [RequireComponent(typeof(Canvas), typeof(CanvasGroup))]
    public abstract class UICanvas : MonoBehaviour
    {
        private const string UI_LAYER_NAME = "UI";

        [Tab("References")]
        [SerializeField, SelfFill] protected Canvas           canvas;
        [SerializeField, SelfFill] protected CanvasGroup      canvasGroup;
        [SerializeField, SelfFill] protected PlayableDirector _playableDirector;

        [Tab("Settings")]
        [SerializeField] protected int           _sortingOrder;
        [SerializeField] protected BgType        _bgType = BgType.NONE;
        [SerializeField] protected PlayableAsset _openTimeline;
        [SerializeField] protected float         _openLockDuration;
        [SerializeField] protected PlayableAsset _closeTimeline;
        [SerializeField] protected float         _closeLockDuration;

        public BgType BgType => _bgType;
        public float OpenLockDuration => _openLockDuration;
        public float CloseLockDuration => _closeLockDuration;

        public virtual void Setup()
        {
            transform.SetAsLastSibling();
            gameObject.SetActive(true);

            canvas.overrideSorting  = true;
            canvas.sortingLayerName = UI_LAYER_NAME;
            canvas.sortingOrder     = _sortingOrder;
        }

        public virtual void Open()
        {
            _playableDirector.playableAsset = _openTimeline;
            _playableDirector.Play();
        }

        public virtual void Close()
        {
            _playableDirector.playableAsset = _closeTimeline;
            _playableDirector.Play();
        }

        public virtual void CloseDirectly()
        {
            gameObject.SetActive(false);
        }
    }
}
