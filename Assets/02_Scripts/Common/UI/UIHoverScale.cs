using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIHoverScale : MonoBehaviour
    , IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    [Header("Scale Animation")]
    [SerializeField] private RectTransform _scaleTarget;
    [SerializeField] private float _hoverScale = 1.08f;
    [SerializeField] private float _pressScale = 0.92f;
    [SerializeField] private float _scaleDuration = 0.12f;

    private Selectable _selectable;
    private Vector3 _originScale = Vector3.one;
    private bool _isHovering = false;

    protected virtual void Awake()
    {
        InitScaleTarget();
    }

    protected virtual void OnEnable()
    {
        _isHovering = false;
        ResetScale();
    }

    protected virtual void OnDisable()
    {
        _isHovering = false;
        ResetScale();
    }

    private void InitScaleTarget()
    {
        if (_scaleTarget == null)
        {
            _scaleTarget = transform as RectTransform;
        }

        if (_scaleTarget != null)
        {
            _originScale = _scaleTarget.localScale;
        }

        if (_selectable == null)
        {
            _selectable = GetComponentInChildren<Selectable>();
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (IsAnimatable() == false)
        {
            return;
        }

        _isHovering = true;
        ScaleTo(_hoverScale, Ease.OutQuad);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (IsAnimatable() == false)
        {
            return;
        }

        _isHovering = false;
        ScaleTo(1f, Ease.OutQuad);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (IsAnimatable() == false)
        {
            return;
        }

        ScaleTo(_pressScale, Ease.OutQuad);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (IsAnimatable() == false)
        {
            return;
        }

        float target = 1f;

        if (_isHovering == true)
        {
            target = _hoverScale;
        }

        ScaleTo(target, Ease.OutBack);
    }

    private bool IsAnimatable()
    {
        if (_scaleTarget == null)
        {
            return false;
        }

        if (_selectable != null && _selectable.interactable == false)
        {
            return false;
        }

        return true;
    }

    private void ScaleTo(float ratio, Ease ease)
    {
        _scaleTarget.DOKill();

        _scaleTarget.DOScale(_originScale * ratio, _scaleDuration).SetEase(ease)
            .SetUpdate(true);
    }

    private void ResetScale()
    {
        if (_scaleTarget == null)
        {
            return;
        }

        _scaleTarget.DOKill();
        _scaleTarget.localScale = _originScale;
    }
}
