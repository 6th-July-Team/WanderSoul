using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public class ScreenFadeUIView : BaseUI
{
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private float _fadeOutDuration = 0.35f;
    [SerializeField] private float _fadeInDuration = 0.45f;

    private Tween _tween;

    protected override void OnOpened()
    {
        SetAlpha(0f);
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
