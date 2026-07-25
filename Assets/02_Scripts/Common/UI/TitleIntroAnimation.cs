using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class TitleIntroAnimation : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private RectTransform _logo;
    [SerializeField] private RectTransform _distantLandscape;
    [SerializeField] private Image _distantLandscapeImage;
    [SerializeField] private RectTransform _foreground;
    [SerializeField] private Image _foregroundImage;
    [SerializeField] private Image _blackScreen;
    [SerializeField] private TitleBarrierShimmer _barrierShimmer;
    [SerializeField] private TitleLogoGlow _logoGlow;
    [SerializeField] private TitleLogoParticles _logoParticles;

    [Header("Start Offset")]
    [SerializeField] private Vector2 _logoStartOffset = Vector2.zero;
    [SerializeField] private Vector3 _logoStartScale = new Vector3(0.75f, 0.75f, 1f);
    [SerializeField] private Vector2 _distantLandscapeStartOffset = new Vector2(0f, -120f);
    [SerializeField] private Vector2 _foregroundStartOffset = new Vector2(-80f, -120f);

    [Header("Duration")]
    [SerializeField] private float _logoDuration = 2.5f;
    [SerializeField] private float _landscapeDuration = 2.2f;
    [SerializeField] private float _foregroundDuration = 2.4f;
    [SerializeField] private float _blackScreenDuration = 2.5f;

    private Vector2 _logoEndPosition;
    private Vector3 _logoEndScale;
    private Vector2 _distantLandscapeEndPosition;
    private Vector2 _foregroundEndPosition;
    private float _blackScreenStartAlpha;
    private Sequence _introSequence;
    private bool _isInitialized;

    private void Awake()
    {
        Initialize();
    }

    private void OnEnable()
    {
        if (!_isInitialized)
        {
            Initialize();
        }

        if (!_isInitialized)
        {
            return;
        }

        PlayIntro();
    }

    private void OnDisable()
    {
        _introSequence?.Kill();
        _introSequence = null;
    }

    private void Initialize()
    {
        if (_logo == null ||
            _distantLandscape == null ||
            _distantLandscapeImage == null ||
            _foreground == null ||
            _foregroundImage == null ||
            _blackScreen == null ||
            _barrierShimmer == null ||
            _logoGlow == null ||
            _logoParticles == null)
        {
            Debug.LogWarning("TitleIntroAnimation의 UI 참조 연결을 확인해 주세요.", this);
            enabled = false;
            return;
        }

        _logoEndPosition = _logo.anchoredPosition;
        _logoEndScale = _logo.localScale;
        _distantLandscapeEndPosition = _distantLandscape.anchoredPosition;
        _foregroundEndPosition = _foreground.anchoredPosition;
        _blackScreenStartAlpha = _blackScreen.color.a;
        _isInitialized = true;
    }

    private void PlayIntro()
    {
        _introSequence?.Kill();

        SetStartState();

        _introSequence = DOTween.Sequence()
            .SetUpdate(true)
            .SetLink(gameObject, LinkBehaviour.KillOnDisable);

        _introSequence.Join(
            _logo.DOAnchorPos(_logoEndPosition, _logoDuration)
                .SetEase(Ease.InOutSine));
        _introSequence.Join(
            _logo.DOScale(_logoEndScale, _logoDuration)
                .SetEase(Ease.InOutSine));

        _introSequence.Join(
            _distantLandscape.DOAnchorPos(_distantLandscapeEndPosition, _landscapeDuration)
                .SetEase(Ease.OutCubic));
        _introSequence.Join(
            _distantLandscapeImage.DOColor(Color.white, _landscapeDuration)
                .SetEase(Ease.OutSine));

        _introSequence.Join(
            _foreground.DOAnchorPos(_foregroundEndPosition, _foregroundDuration)
                .SetEase(Ease.OutCubic));
        _introSequence.Join(
            _foregroundImage.DOColor(Color.white, _foregroundDuration)
                .SetEase(Ease.OutSine));

        _introSequence.Join(
            _blackScreen.DOFade(0f, _blackScreenDuration)
                .SetEase(Ease.OutSine));

        _introSequence.OnComplete(OnIntroComplete);
    }

    private void SetStartState()
    {
        Canvas.ForceUpdateCanvases();

        _logo.anchoredPosition = GetCenteredPosition(_logo) + _logoStartOffset;
        _logo.localScale = _logoStartScale;
        _distantLandscape.anchoredPosition = _distantLandscapeEndPosition + _distantLandscapeStartOffset;
        _foreground.anchoredPosition = _foregroundEndPosition + _foregroundStartOffset;

        SetRgb(_distantLandscapeImage, 0f);
        SetRgb(_foregroundImage, 0f);

        Color blackScreenColor = _blackScreen.color;
        blackScreenColor.a = _blackScreenStartAlpha;
        _blackScreen.color = blackScreenColor;
        _blackScreen.raycastTarget = true;
        _barrierShimmer.StopAnimation();
        _logoGlow.StopAnimation();
        _logoParticles.StopAnimation();
    }

    private Vector2 GetCenteredPosition(RectTransform target)
    {
        RectTransform parent = target.parent as RectTransform;

        if (parent == null)
        {
            return Vector2.zero;
        }

        Vector2 anchorPosition = new Vector2(
            Mathf.Lerp(parent.rect.xMin, parent.rect.xMax, target.anchorMin.x),
            Mathf.Lerp(parent.rect.yMin, parent.rect.yMax, target.anchorMin.y));

        Vector2 centerOffset = new Vector2(
            (0.5f - target.pivot.x) * target.rect.width,
            (0.5f - target.pivot.y) * target.rect.height);

        return parent.rect.center - centerOffset - anchorPosition;
    }

    private void SetRgb(Image image, float rgb)
    {
        Color color = image.color;
        color.r = rgb;
        color.g = rgb;
        color.b = rgb;
        image.color = color;
    }

    private void OnIntroComplete()
    {
        _blackScreen.raycastTarget = false;
        _barrierShimmer.PlayAnimation();
        _logoGlow.PlayAnimation();
        _logoParticles.PlayAnimation();
        _introSequence = null;
    }
}
