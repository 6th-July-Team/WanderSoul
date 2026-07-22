using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WagonAreaWarningUIView : BaseUI<WagonAreaWarningUIView, WagonViewModel>
{
    [SerializeField] private GameObject _warningPanel;
    [SerializeField] private TMP_Text _countdownText;
    [SerializeField] private Image _vignetteBaseImage;
    [SerializeField] private Image _vignettePulseImage;

    // TODO(이태영): 경고 시간을 데이터에서 가져오기 - WagonBoundary와 값이 중복됨
    [SerializeField] private float _warningDuration = 10f;

    [Header("Base Fade")]
    [SerializeField] private float _fadeInDuration = 0.6f;
    [SerializeField] private float _startAlpha = 0.15f;
    [SerializeField] private float _endAlpha = 0.7f;

    [Header("Pulse")]
    [SerializeField] private float _pulseAlpha = 0.25f;
    [SerializeField] private float _pulseDuration = 0.9f;
    [SerializeField] private float _pulseSpeedRate = 1.5f;

    private Tween _fadeTween;
    private Tween _pulseTween;

    private void OnDisable()
    {
        StopAllTween();
    }

    protected override void OnPropertyChanged(string propertyName)
    {
        if (propertyName == nameof(WagonModel.WarningTime))
        {
            RefreshWarning();
        }
    }

    private void RefreshWarning()
    {
        if (_viewModel == null)
        {
            return;
        }

        float elapsed = _viewModel.GetWarningTime;
        bool isActive = (elapsed > 0f);

        if (isActive == false)
        {
            _warningPanel.SetActive(false);
            StopAllTween();
            return;
        }

        if (_warningPanel.activeSelf == false)
        {
            _warningPanel.SetActive(true);
            PlayWarningStart();
            return;
        }

        float remaining = _warningDuration - elapsed;

        if (remaining < 0f)
        {
            remaining = 0f;
        }

        _countdownText.text = $"{remaining:F0}";

        RefreshByProgress(elapsed);
    }

    private void PlayWarningStart()
    {
        StopAllTween();

        if (_vignetteBaseImage != null)
        {
            SetImageAlpha(_vignetteBaseImage, 0f);
            _fadeTween = _vignetteBaseImage.DOFade(_startAlpha, _fadeInDuration).SetEase(Ease.OutSine);
        }

        if (_vignettePulseImage != null)
        {
            SetImageAlpha(_vignettePulseImage, 0f);
            _pulseTween = _vignettePulseImage.DOFade(_pulseAlpha, _pulseDuration)
                .SetLoops(-1, LoopType.Yoyo)
                .SetEase(Ease.InOutSine);
        }
    }

    private void RefreshByProgress(float elapsed)
    {
        float ratio = Mathf.Clamp01(elapsed / _warningDuration);

        RefreshBaseAlpha(ratio);
        RefreshPulseSpeed(ratio);
    }

    private void RefreshBaseAlpha(float ratio)
    {
        if (_vignetteBaseImage == null)
        {
            return;
        }

        // 페이드 인이 끝나기 전에는 알파를 건드리지 않는다
        if (_fadeTween != null && _fadeTween.IsActive() == true)
        {
            return;
        }

        float alpha = Mathf.Lerp(_startAlpha, _endAlpha, ratio);
        SetImageAlpha(_vignetteBaseImage, alpha);
    }

    private void RefreshPulseSpeed(float ratio)
    {
        if (_pulseTween == null)
        {
            return;
        }

        _pulseTween.timeScale = 1f + (ratio * _pulseSpeedRate);
    }

    private void SetImageAlpha(Image image, float alpha)
    {
        if (image == null)
        {
            return;
        }

        Color color = image.color;
        color.a = alpha;
        image.color = color;
    }

    private void StopAllTween()
    {
        if (_fadeTween != null)
        {
            _fadeTween.Kill();
            _fadeTween = null;
        }

        if (_pulseTween != null)
        {
            _pulseTween.Kill();
            _pulseTween = null;
        }

        if (_vignetteBaseImage != null)
        {
            _vignetteBaseImage.DOKill();
        }

        if (_vignettePulseImage != null)
        {
            _vignettePulseImage.DOKill();
        }
    }

}
