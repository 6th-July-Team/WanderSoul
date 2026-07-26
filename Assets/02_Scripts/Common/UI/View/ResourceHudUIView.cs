using System;
using TMPro;
using UnityEngine;

public class ResourceHudUIView : BaseUI<ResourceHudUIView, ResourceHudViewModel>
{

    [SerializeField] private TMP_Text _soulText;
    [SerializeField] private TMP_Text _moneyText;

    private PlayerOutGameViewModel _outGameViewModel;
    private Action<string> _soulHandler;

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
        if (propertyName == nameof(ResourceModel.Money))
        {
            _moneyText.text = $"{_viewModel.Money:N0}";
        }
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

    private void OnSoulPropertyChanged(string propertyName)
    {
        if (propertyName == nameof(PlayerOutGameModel.Soul))
        {
            RefreshSoul();
        }
    }

    private void RefreshSoul()
    {
        _soulText.text = $"{_outGameViewModel.GetSoul:N0}";
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
