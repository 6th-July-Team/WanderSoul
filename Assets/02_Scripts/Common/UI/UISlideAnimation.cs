using DG.Tweening;
using UnityEngine;

public class UISlideAnimation : MonoBehaviour
{
    public enum SlideDirection
    {
        Up,
        Down,
        Left,
        Right,
    }

    [SerializeField] private RectTransform _targetRect;
    [SerializeField] private SlideDirection _direction;
    [SerializeField] private float _slideDistance = 300f;
    [SerializeField] private float _duration = 0.3f;

    private Vector2 _originPos;
    private bool _isCached = false;

    private void OnEnable()
    {
        GameManager.UI.RegisterHudAnimation(this);
    }

    private void OnDisable()
    {
        GameManager.UI.UnregisterHudAnimation(this);
    }

    private void CacheOriginPosition()
    {
        if (_isCached == true)
        {
            return;
        }

        _originPos = _targetRect.anchoredPosition;
        _isCached = true;
    }

    private Vector2 GetHiddenPosition()
    {
        Vector2 offset = Vector2.zero;

        switch (_direction)
        {
            case SlideDirection.Up:
                offset = new Vector2(0f, _slideDistance);
                break;

            case SlideDirection.Down:
                offset = new Vector2(0f, -_slideDistance);
                break;

            case SlideDirection.Left:
                offset = new Vector2(-_slideDistance, 0f);
                break;

            case SlideDirection.Right:
                offset = new Vector2(_slideDistance, 0f);
                break;
        }

        return _originPos + offset;
    }

    public void SlideOut()
    {
        CacheOriginPosition();

        _targetRect.DOKill();
        _targetRect.DOAnchorPos(GetHiddenPosition(), _duration).SetEase(Ease.InCubic);
    }

    public void SlideIn()
    {
        CacheOriginPosition();

        _targetRect.DOKill();
        _targetRect.DOAnchorPos(_originPos, _duration).SetEase(Ease.OutCubic);
    }

    private void OnDestroy()
    {
        if (_targetRect != null)
        {
            _targetRect.DOKill();
        }
    }
}
