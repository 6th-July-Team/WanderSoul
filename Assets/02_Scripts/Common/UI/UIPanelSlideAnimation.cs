using DG.Tweening;
using UnityEngine;

public class UIPanelSlideAnimation : MonoBehaviour
{
    [SerializeField] private CanvasGroup _dimCanvasGroup;
    [SerializeField] private RectTransform _topRect;
    [SerializeField] private RectTransform _leftRect;
    [SerializeField] private RectTransform _rightRect;
    [SerializeField] private float _duration = 0.25f;
    [SerializeField] private float _slideDistance = 200f;

    private Vector2 _topOriginPos;
    private Vector2 _leftOriginPos;
    private Vector2 _rightOriginPos;
    private bool _isPositionCached = false;

    private void CacheOriginPositions()
    {
        if (_isPositionCached == true)
        {
            return;
        }

        _topOriginPos = _topRect.anchoredPosition;
        _leftOriginPos = _leftRect.anchoredPosition;
        _rightOriginPos = _rightRect.anchoredPosition;
        _isPositionCached = true;
    }

    public void PlayOpen()
    {
        CacheOriginPositions();

        _dimCanvasGroup.DOKill();
        _dimCanvasGroup.alpha = 0f;
        _dimCanvasGroup.DOFade(1f, _duration);

        _topRect.DOKill();
        _topRect.anchoredPosition = _topOriginPos + new Vector2(0f, _slideDistance);
        _topRect.DOAnchorPos(_topOriginPos, _duration).SetEase(Ease.OutCubic);

        _leftRect.DOKill();
        _leftRect.anchoredPosition = _leftOriginPos + new Vector2(-_slideDistance, 0f);
        _leftRect.DOAnchorPos(_leftOriginPos, _duration).SetEase(Ease.OutCubic);

        _rightRect.DOKill();
        _rightRect.anchoredPosition = _rightOriginPos + new Vector2(_slideDistance, 0f);
        _rightRect.DOAnchorPos(_rightOriginPos, _duration).SetEase(Ease.OutCubic);
    }

    public void PlayClose(TweenCallback onComplete = null)
    {
        CacheOriginPositions();

        _dimCanvasGroup.DOKill();
        _dimCanvasGroup.DOFade(0f, _duration);

        _topRect.DOKill();
        _topRect.DOAnchorPos(_topOriginPos + new Vector2(0f, _slideDistance), _duration).SetEase(Ease.InCubic);

        _leftRect.DOKill();
        _leftRect.DOAnchorPos(_leftOriginPos + new Vector2(-_slideDistance, 0f), _duration).SetEase(Ease.InCubic);

        _rightRect.DOKill();
        Tween tween = _rightRect.DOAnchorPos(_rightOriginPos + new Vector2(_slideDistance, 0f), _duration).SetEase(Ease.InCubic);

        if (onComplete != null)
        {
            tween.OnComplete(onComplete);
        }
    }

    private void OnDestroy()
    {
        if (_dimCanvasGroup != null) _dimCanvasGroup.DOKill();
        if (_topRect != null) _topRect.DOKill();
        if (_leftRect != null) _leftRect.DOKill();
        if (_rightRect != null) _rightRect.DOKill();
    }
}