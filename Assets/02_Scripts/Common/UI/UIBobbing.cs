using DG.Tweening;
using UnityEngine;

public class UIBobbing : MonoBehaviour
{
    [SerializeField] private float _height = 5f;
    [SerializeField] private float _duration = 0.3f;

    private RectTransform _rectTransform;
    private float _originY;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        _originY = _rectTransform.anchoredPosition.y;
    }

    private void OnEnable()
    {
        _rectTransform.DOAnchorPosY(_originY + _height, _duration)
            .SetLoops(-1, LoopType.Yoyo)
            .SetEase(Ease.InOutSine);
    }

    private void OnDisable()
    {
        _rectTransform.DOKill();
        Vector2 pos = _rectTransform.anchoredPosition;
        pos.y = _originY;
        _rectTransform.anchoredPosition = pos;
    }
}