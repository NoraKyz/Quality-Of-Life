using Quality.Core.CustomAttribute;
using UnityEngine;

namespace Quality.Core.MiUI
{
    [RequireComponent(typeof(Canvas), typeof(CanvasGroup))]
    public abstract class UICanvas : MonoBehaviour
    {
        [SerializeField, SelfFill] protected Canvas      canvas;
        [SerializeField, SelfFill] protected CanvasGroup canvasGroup;

        [SerializeField] private Animator _animator;

        // public bool CanBack { get; private set; }
        // public IReadOnlyUIData Data { get; private set; }
        //
        //
        // public void Init(UIData data, bool canBack)
        // {
        //     Data    = data;
        //     CanBack = canBack;
        // }
        //
        // public virtual void Open()
        // {
        //     transform.SetAsLastSibling();
        //     gameObject.SetActive(true);
        //
        //     canvas.overrideSorting = true;
        //     canvas.sortingOrder    = Data.SortingOrder;
        //     canvasGroup.alpha      = 0f;
        //     Tween.Alpha(canvasGroup, 1f, 0.3f, Ease.OutCubic);
        // }
        //
        // public virtual void Close()
        // {
        //     Tween.Alpha(canvasGroup, 0f, Data.CloseDuration, Ease.InCubic);
        // }
        //
        // public virtual void CloseDirectly()
        // {
        //     gameObject.SetActive(false);
        // }
    }
}
