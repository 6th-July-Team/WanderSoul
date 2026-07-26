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

    // TODO(이태영): 레벨/요구 경험치 테이블이 생기면 PlayerViewModel에서 가져오도록 교체
    private const float TEMP_MAX_EXP = 100f;


    [Header("Animation")]
    [SerializeField] private UISlideAnimation _slideAnimation;

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

        else if (propertyName == nameof(PlayerModel.EXP))
        {
            RefreshExp();
        }
    }

    protected override void OnOpened()
    {
        _slideAnimation.SetHidden();
        _slideAnimation.SlideIn();
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

    // 오브 셰이더가 액체 높이를 그리므로, 오브가 있으면 fillAmount는 건드리지 않는다.
    // (같은 Image를 공유해서 둘 다 적용하면 이중으로 잘린다)
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
        if (_viewModel == null)
        {
            return;
        }

        float maxExp = TEMP_MAX_EXP;

        if (maxExp <= 0f)
        {
            return;
        }

        float exp = _viewModel.GetExp;

        if (_expSlider != null)
        {
            _expSlider.value = Mathf.Clamp01(exp / maxExp);
        }

        if (_expText != null)
        {
            _expText.text = $"{(int)exp:N0} / {(int)maxExp:N0}";
        }
    }
}
