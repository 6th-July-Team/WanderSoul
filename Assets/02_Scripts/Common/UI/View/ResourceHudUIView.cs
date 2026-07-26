using System;
using TMPro;
using UnityEngine;

public class ResourceHudUIView : BaseUI<ResourceHudUIView, ResourceHudViewModel>
{

    [SerializeField] private TMP_Text _soulText;
    [SerializeField] private TMP_Text _moneyText;

    private PlayerOutGameViewModel _outGameViewModel;
    private Action<string> _outGameHandler;

    [Header("Layout")]
    [SerializeField] private RectTransform _rootRect;
    [SerializeField] private Vector2 _villageAnchoredPosition;
    [SerializeField] private Vector2 _convoyAnchoredPosition;

    [Header("Animation")]
    [SerializeField] private UISlideAnimation _slideAnimation;

    protected override void OnOpened()
    {
        if (_slideAnimation == null)
        {
            return;
        }

        _slideAnimation.SetHidden();
        _slideAnimation.SlideIn();
    }

    public void SetVillageLayout()
    {
        if (_rootRect == null)
        {
            return;
        }

        _rootRect.anchoredPosition = _villageAnchoredPosition;
    }
    public void SetConvoyLayout()
    {
        if (_rootRect == null)
        {
            return;
        }

        _rootRect.anchoredPosition = _convoyAnchoredPosition;
    }

    protected override void OnPropertyChanged(string propertyName)
    {
    }

    public void SetOutGameSource(PlayerOutGameViewModel outGameViewModel)
    {
        UnbindOutGame();

        if (outGameViewModel == null)
        {
            return;
        }

        _outGameViewModel = outGameViewModel;
        _outGameHandler = (propertyName) => OnOutGamePropertyChanged(propertyName);
        _outGameViewModel.OnPropertyChanged_View += _outGameHandler;

        RefreshSoul();
        RefreshMoney();
    }

    private void OnOutGamePropertyChanged(string propertyName)
    {
        if (propertyName == nameof(PlayerOutGameModel.Soul))
        {
            RefreshSoul();
        }

        else if (propertyName == nameof(PlayerOutGameModel.Gold))
        {
            RefreshMoney();
        }
    }

    private void RefreshSoul()
    {
        _soulText.text = $"{_outGameViewModel.GetSoul:N0}";
    }

    private void RefreshMoney()
    {
        _moneyText.text = $"{_outGameViewModel.GetGold:N0}";
    }

    private void UnbindOutGame()
    {
        if (_outGameViewModel == null)
        {
            return;
        }

        _outGameViewModel.OnPropertyChanged_View -= _outGameHandler;
        _outGameViewModel = null;
        _outGameHandler = null;
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        UnbindOutGame();
    }
}
