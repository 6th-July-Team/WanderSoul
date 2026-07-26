using System;
using TMPro;
using UnityEngine;

public class MagicCircleUIView : BaseUI<MagicCircleUIView, MagicCircleViewModel>
{
    [Header("Soul")]
    [SerializeField] private TMP_Text _soulText;

    [Header("Tab")]
    [SerializeField] private UIButton _summonTabButton;
    [SerializeField] private UIButton _resetTabButton;
    [SerializeField] private GameObject _summonPanel;
    [SerializeField] private GameObject _resetPanel;

    [Header("Summon")]
    [SerializeField] private TMP_Text _summonOneCostText;
    [SerializeField] private TMP_Text _summonFiveCostText;
    [SerializeField] private UIButton _summonOneButton;
    [SerializeField] private UIButton _summonFiveButton;

    [Header("Reset")]
    [SerializeField] private TMP_Text _resetInfoText;
    [SerializeField] private UIButton _resetButton;

    [Header("Common")]
    [SerializeField] private UIButton _closeButton;
    [SerializeField] private UISlideAnimation _slideAnimation;

    private PlayerOutGameViewModel _outGameViewModel;
    private Action<string> _soulHandler;

    private bool _isResetConfirmed = false;

    protected override void OnInit()
    {
        _summonTabButton.BindOnClickButtonEvent(ShowSummonTab);
        _resetTabButton.BindOnClickButtonEvent(ShowResetTab);

        _summonOneButton.BindOnClickButtonEvent(OnClickSummonOne);
        _summonFiveButton.BindOnClickButtonEvent(OnClickSummonFive);
        _resetButton.BindOnClickButtonEvent(OnClickReset);

        if (_closeButton != null)
        {
            _closeButton.BindOnClickButtonEvent(OnClickClose);
        }
    }

    protected override void OnOpened()
    {
        ShowSummonTab();

        if (_slideAnimation == null)
        {
            return;
        }

        _slideAnimation.SetHidden();
        _slideAnimation.SlideIn();
    }

    public void SetSoulSource(PlayerOutGameViewModel outGameViewModel)
    {
        UnbindSoul();

        if (outGameViewModel == null)
        {
            return;
        }

        _outGameViewModel = outGameViewModel;
        _soulHandler = (propertyName) => OnSoulPropertyChanged(propertyName);
        _outGameViewModel.OnPropertyChanged_View += _soulHandler;

        RefreshSoul();
    }

    protected override void OnPropertyChanged(string propertyName)
    {
        if (propertyName == nameof(MagicCircleModel.SummonOneCost)
            || propertyName == nameof(MagicCircleModel.SummonFiveCost))
        {
            RefreshCosts();
        }
    }

    private void RefreshCosts()
    {
        _summonOneCostText.text = $"{_viewModel.SummonOneCost:N0}";
        _summonFiveCostText.text = $"{_viewModel.SummonFiveCost:N0}";
    }

    #region Tab

    private void ShowSummonTab()
    {
        _summonPanel.SetActive(true);
        _resetPanel.SetActive(false);
    }

    private void ShowResetTab()
    {
        _summonPanel.SetActive(false);
        _resetPanel.SetActive(true);

        _isResetConfirmed = false;
    }

    #endregion

    #region Summon

    private void OnClickSummonOne()
    {
        TrySummon(1);
    }

    private void OnClickSummonFive()
    {
        TrySummon(5);
    }

    private void TrySummon(int summonCount)
    {
        if (_viewModel.TrySummon(summonCount) == false)
        {
            // TODO(태영): 소환 실패 처리(소울 부족 팝업 등)
            return;
        }

        // TODO(태영): 소환 성공 결과 표시
    }

    #endregion

    #region Reset

    private void OnClickReset()
    {
        if (_isResetConfirmed == false)
        {
            GameManager.UI.OpenSimplePopup("초기화하면 능력치 카드와 궁극기가 모두 사라집니다. 한 번 더 누르면 초기화됩니다.");
            _isResetConfirmed = true;
            return;
        }

        _isResetConfirmed = false;
        _viewModel.ResetLevel();
    }

    #endregion

    private void OnClickClose()
    {
        GameManager.UI.CloseUI(UIType.MagicCircleUIView);
    }

    private void RefreshSoul()
    {
        _soulText.text = $"{_outGameViewModel.GetSoul:N0}";
    }

    private void OnSoulPropertyChanged(string propertyName)
    {
        if (propertyName == nameof(PlayerOutGameModel.Soul))
        {
            RefreshSoul();
        }
    }

    private void UnbindSoul()
    {
        if (_outGameViewModel == null)
        {
            return;
        }

        _outGameViewModel.OnPropertyChanged_View -= _soulHandler;
        _outGameViewModel = null;
        _soulHandler = null;
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        UnbindSoul();
    }
}
