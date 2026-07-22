using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHudUIView : BaseUI<PlayerHudUIView, PlayerViewModel>
{

    [Header("HP")]
    [SerializeField] private Image _hpFillImage;
    [SerializeField] private TMP_Text _hpText;

    [Header("Mana")]
    [SerializeField] private Image _manaFillImage;
    [SerializeField] private TMP_Text _manaText;

    [Header("Exp")]
    [SerializeField] private Slider _expSlider;
    [SerializeField] private TMP_Text _expText;


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

        // TODO(이태영): PlayerModel에 Exp/Level 추가되면 주석 해제
        // else if (propertyName == nameof(PlayerModel.Exp) || propertyName == nameof(PlayerModel.Level))
        // {
        //     RefreshExp();
        // }
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

        _hpFillImage.fillAmount = _viewModel.GetHp / maxHp;
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

        _manaFillImage.fillAmount = _viewModel.GetMp / maxMp;
        _manaText.text = $"{(int)_viewModel.GetMp:N0} / {(int)maxMp:N0}";
    }

    private void RefreshExp()
    {
        if (_viewModel == null)
        {
            return;
        }

        // TODO(이태영): PlayerViewModel에 GetExp/GetMaxExp/GetLevel 추가되면 연결
        // float maxExp = _viewModel.GetMaxExp;
        // if (maxExp <= 0f)
        // {
        //     _expSlider.value = 0f;
        //     return;
        // }
        //
        // _expSlider.value = _viewModel.GetExp / maxExp;
        //
        // if (_expText != null)
        // {
        //     _expText.text = $"Lv.{_viewModel.GetLevel}";
        // }
    }
}
