using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class OptionUI : BaseUI
{
    [Header("Volume")]
    [SerializeField] private Slider _bgmSlider;
    [SerializeField] private Slider _sfxSlider;
    [SerializeField] private Toggle _muteToggle;
    [SerializeField] private Image _muteIconImage;
    [SerializeField] private Sprite _soundOnSprite;
    [SerializeField] private Sprite _soundOffSprite;

    [Header("Gameplay")]
    [SerializeField] private Toggle _autoAttackToggle;

    [Header("Button")]
    [SerializeField] private UIButton _closeButton;

    [Header("Animation")]
    [SerializeField] private UISlideAnimation _slideAnimation;
    [SerializeField] private CanvasGroup _fadeCanvasGroup;
    [SerializeField] private float _fadeDuration = 0.3f;

    private bool _isClosing = false;

    private const string KEY_BGM = "Option.BgmVolume";
    private const string KEY_SFX = "Option.SfxVolume";
    private const string KEY_MUTE = "Option.Mute";

    private float _bgmVolume = 0f;
    private float _sfxVolume = 0f;
    private bool _isMute = false;

    private bool _isRefreshing = false;

    protected override void OnInit()
    {
        if (_closeButton != null)
        {
            _closeButton.BindOnClickButtonEvent(OnClickClose);
        }

        if (_bgmSlider != null)
        {
            _bgmSlider.onValueChanged.AddListener(OnBgmSliderChanged);
        }

        if (_sfxSlider != null)
        {
            _sfxSlider.onValueChanged.AddListener(OnSfxSliderChanged);
        }

        if (_muteToggle != null)
        {
            _muteToggle.onValueChanged.AddListener(OnMuteToggleChanged);
        }

        if (_autoAttackToggle != null)
        {
            _autoAttackToggle.onValueChanged.AddListener(OnAutoAttackToggleChanged);
        }

        LoadSavedValue();
    }

    protected override void OnOpened()
    {
        GameManager.Time.OnPause();
        RefreshSliders();
        PlayOpenAnimation();
    }

    protected override void OnClosed()
    {
        GameManager.Time.OnResume();
    }

    private void OnDestroy()
    {
        if (_fadeCanvasGroup != null)
        {
            _fadeCanvasGroup.DOKill();
        }
    }

    private void PlayOpenAnimation()
    {
        _isClosing = false;

        if (_fadeCanvasGroup != null)
        {
            _fadeCanvasGroup.DOKill();
            _fadeCanvasGroup.alpha = 0f;
            _fadeCanvasGroup.DOFade(1f, _fadeDuration);
        }

        if (_slideAnimation == null)
        {
            return;
        }

        _slideAnimation.SetHidden();
        _slideAnimation.SlideIn();
    }

    private void PlayCloseAnimation()
    {
        if (_fadeCanvasGroup != null)
        {
            _fadeCanvasGroup.DOKill();
            _fadeCanvasGroup.DOFade(0f, _fadeDuration);
        }

        _slideAnimation.SlideOut(CloseSelf);
    }

    private void CloseSelf()
    {
        GameManager.UI.CloseUI(UIType.OptionUI);
    }

    private void LoadSavedValue()
    {
        _bgmVolume = PlayerPrefs.GetFloat(KEY_BGM, GameManager.Sound.GetBGMVolume());
        _sfxVolume = PlayerPrefs.GetFloat(KEY_SFX, GameManager.Sound.GetSFXVolume());
        _isMute = (PlayerPrefs.GetInt(KEY_MUTE, 0) == 1);

        ApplyVolume();
    }

    private void RefreshSliders()
    {
        _isRefreshing = true;

        if (_bgmSlider != null)
        {
            _bgmSlider.value = _bgmVolume;
        }

        if (_sfxSlider != null)
        {
            _sfxSlider.value = _sfxVolume;
        }

        if (_muteToggle != null)
        {
            _muteToggle.isOn = _isMute;
        }

        if (_autoAttackToggle != null)
        {
            _autoAttackToggle.isOn = GameOption.IsAutoAttack;
        }

        _isRefreshing = false;

        RefreshMuteIcon();
    }

    private void RefreshMuteIcon()
    {
        if (_muteIconImage == null)
        {
            return;
        }

        Sprite sprite = _soundOnSprite;

        if (_isMute == true)
        {
            sprite = _soundOffSprite;
        }

        if (sprite == null)
        {
            return;
        }

        _muteIconImage.sprite = sprite;
    }

    private void OnBgmSliderChanged(float value)
    {
        if (_isRefreshing == true)
        {
            return;
        }

        _bgmVolume = Mathf.Clamp01(value);
        PlayerPrefs.SetFloat(KEY_BGM, _bgmVolume);
        PlayerPrefs.Save();

        ApplyVolume();
    }

    private void OnSfxSliderChanged(float value)
    {
        if (_isRefreshing == true)
        {
            return;
        }

        _sfxVolume = Mathf.Clamp01(value);
        PlayerPrefs.SetFloat(KEY_SFX, _sfxVolume);
        PlayerPrefs.Save();

        ApplyVolume();
    }

    private void OnMuteToggleChanged(bool isOn)
    {
        if (_isRefreshing == true)
        {
            return;
        }

        _isMute = isOn;

        if (_isMute == true)
        {
            PlayerPrefs.SetInt(KEY_MUTE, 1);
        }

        else
        {
            PlayerPrefs.SetInt(KEY_MUTE, 0);
        }

        PlayerPrefs.Save();

        ApplyVolume();
        RefreshMuteIcon();
    }

    private void OnAutoAttackToggleChanged(bool isOn)
    {
        if (_isRefreshing == true)
        {
            return;
        }

        GameOption.SetAutoAttack(isOn);
    }

    private void ApplyVolume()
    {
        float bgm = _bgmVolume;
        float sfx = _sfxVolume;

        if (_isMute == true)
        {
            bgm = 0f;
            sfx = 0f;
        }

        GameManager.Sound.SetBGMVolume(bgm);
        GameManager.Sound.SetSFXVolume(sfx);
    }

    public void Close()
    {
        OnClickClose();
    }

    private void OnClickClose()
    {
        if (_isClosing == true)
        {
            return;
        }

        if (_slideAnimation == null)
        {
            CloseSelf();
            return;
        }

        _isClosing = true;
        PlayCloseAnimation();
    }
}
