using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHudUIView : BaseUI<PlayerHudUIView, PlayerViewModel>
{

    [Header("HP")]
    [SerializeField] private Image _hpFillImage;
    [SerializeField] private UIOrbLiquid _hpOrbLiquid;
    [SerializeField] private TMP_Text _hpText;

    [Header("Mana")]
    [SerializeField] private Image _manaFillImage;
    [SerializeField] private UIOrbLiquid _manaOrbLiquid;
    [SerializeField] private TMP_Text _manaText;

    [Header("Exp")]
    [SerializeField] private Slider _expSlider;
    [SerializeField] private TMP_Text _expText;



    [Header("Animation")]
    [SerializeField] private UISlideAnimation _slideAnimation;

    private PlayerOutGameViewModel _outGameViewModel;

    public void BindOutGameViewModel(PlayerOutGameViewModel outGameViewModel)
    {
        UnbindOutGameViewModel();

        _outGameViewModel = outGameViewModel;

        if (_outGameViewModel == null)
        {
            return;
        }
        
        _outGameViewModel.OnPropertyChanged_View += OnOutGamePropertyChanged;

        RefreshExp();
    }

    private void UnbindOutGameViewModel()
    {
        if (_outGameViewModel == null)
        {
            return;
        }

        _outGameViewModel.OnPropertyChanged_View -= OnOutGamePropertyChanged;
        _outGameViewModel = null;
    }

    private void OnOutGamePropertyChanged(string propertyName)
    {
        if (propertyName == nameof(PlayerOutGameModel.Exp))
        {
            RefreshExp();
        }

        else if (propertyName == nameof(PlayerOutGameModel.Level))
        {
            RefreshExp();
        }
    }

    protected override void OnPropertyChanged(string propertyName)
    {
        if (propertyName == nameof(PlayerModel.HP))
        {
            RefreshHp();
        }

        else if (propertyName == nameof(PlayerModel.MP))
        {
            RefreshMana();
        }
    }

    protected override void OnOpened()
    {
        _slideAnimation.SetHidden();
        _slideAnimation.SlideIn();
    }

    protected override void OnClosed()
    {
        UnbindOutGameViewModel();
    }

    private void RefreshHp()
    {
        if (_viewModel == null)
        {
            return;
        }

        float maxHp = _viewModel.MaxHP;
        if (maxHp <= 0f)
        {
            return;
        }

        RefreshOrb(_hpOrbLiquid, _hpFillImage, _viewModel.GetHp / maxHp);
        _hpText.text = $"{(int)_viewModel.GetHp:N0} / {(int)maxHp:N0}";
    }

    private void RefreshMana()
    {
        if (_viewModel == null)
        {
            return;
        }

        float maxMp = _viewModel.MaxMP;
        if (maxMp <= 0f)
        {
            return;
        }

        RefreshOrb(_manaOrbLiquid, _manaFillImage, _viewModel.GetMp / maxMp);
        _manaText.text = $"{(int)_viewModel.GetMp:N0} / {(int)maxMp:N0}";
    }

    private void RefreshOrb(UIOrbLiquid orbLiquid, Image fillImage, float normalizedValue)
    {
        if (orbLiquid != null)
        {
            orbLiquid.SetValue(normalizedValue);
            return;
        }

        if (fillImage != null)
        {
            fillImage.fillAmount = normalizedValue;
        }
    }

    private void RefreshExp()
    {
        if (_outGameViewModel == null)
        {
            return;
        }

        float requiredExp = _outGameViewModel.GetRequiredExp;

        if (requiredExp <= 0f)
        {
            return;
        }

        float exp = _outGameViewModel.GetExp;

        if (_expSlider != null)
        {
            _expSlider.value = Mathf.Clamp01(exp / requiredExp);
        }

        if (_expText != null)
        {
            _expText.text = $"Lv.{_outGameViewModel.GetLevel}";
        }
    }
}
