using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public class ScreenFadeUIView : BaseUI
{
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private float _fadeOutDuration = 0.35f;
    [SerializeField] private float _fadeInDuration = 0.45f;

    [Header("Intro")]
    [SerializeField] private CanvasGroup _logoCanvasGroup;
    [SerializeField] private TitleLogoGlow _logoGlow;
    [SerializeField] private TitleLogoParticles _logoParticles;
    [SerializeField] private float _blackHoldDuration = 1f;
    [SerializeField] private float _logoFadeDuration = 0.8f;
    [SerializeField] private float _logoHoldDuration = 1f;

    private Tween _tween;

    protected override void OnOpened()
    {
        SetAlpha(0f);
        SetLogoAlpha(0f);
    }

    public async UniTask PlayIntroAsync()
    {
        SetLogoAlpha(0f);

        await UniTask.Delay(System.TimeSpan.FromSeconds(_blackHoldDuration), true);

        if (_logoCanvasGroup == null)
        {
            return;
        }

        if (_logoGlow != null)
        {
            _logoGlow.PlayAnimation();
        }

        if (_logoParticles != null)
        {
            _logoParticles.PlayAnimation();
        }

        await _logoCanvasGroup.DOFade(1f, _logoFadeDuration).SetUpdate(true).AsyncWaitForCompletion();

        await UniTask.Delay(System.TimeSpan.FromSeconds(_logoHoldDuration), true);

        if (_logoCanvasGroup == null)
        {
            return;
        }

        await _logoCanvasGroup.DOFade(0f, _logoFadeDuration).SetUpdate(true).AsyncWaitForCompletion();
    }

    private void SetLogoAlpha(float alpha)
    {
        if (Mathf.Approximately(alpha, 0f) == true)
        {
            if (_logoGlow != null)
            {
                _logoGlow.StopAnimation();
            }

            if (_logoParticles != null)
            {
                _logoParticles.StopAnimation();
            }
        }

        if (_logoCanvasGroup == null)
        {
            return;
        }

        _logoCanvasGroup.DOKill();
        _logoCanvasGroup.alpha = alpha;
    }

    private void OnDisable()
    {
        KillTween();
    }

    public void SetBlackImmediate()
    {
        SetAlpha(1f);
    }

    public async UniTask FadeOutAsync()
    {
        await FadeAsync(1f, _fadeOutDuration);
    }

    public async UniTask FadeInAsync()
    {
        await FadeAsync(0f, _fadeInDuration);
    }

    private async UniTask FadeAsync(float targetAlpha, float duration)
    {
        if (_canvasGroup == null)
        {
            return;
        }

        KillTween();

        _canvasGroup.blocksRaycasts = true;

        Tween tween = _canvasGroup.DOFade(targetAlpha, duration)
            .SetEase(Ease.InOutSine)
            .SetUpdate(true);

        _tween = tween;

        await tween.AsyncWaitForCompletion();

        if (_tween == tween)
        {
            _tween = null;
        }

        if (_canvasGroup != null && Mathf.Approximately(targetAlpha, 0f) == true)
        {
            _canvasGroup.blocksRaycasts = false;
            SetLogoAlpha(0f);
        }
    }

    private void SetAlpha(float alpha)
    {
        if (_canvasGroup == null)
        {
            return;
        }

        KillTween();

        _canvasGroup.alpha = alpha;
        _canvasGroup.blocksRaycasts = (alpha > 0f);
    }

    private void KillTween()
    {
        if (_tween == null)
        {
            return;
        }

        _tween.Kill();
        _tween = null;
    }
}
