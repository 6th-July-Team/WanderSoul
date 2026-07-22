using TMPro;
using UnityEngine;

public class ResourceHudUIView : BaseUI<ResourceHudUIView, ResourceHudViewModel>
{

    [SerializeField] private TMP_Text _soulText;
    [SerializeField] private TMP_Text _moneyText;

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
        if (propertyName == nameof(ResourceModel.Soul))
        {
            _soulText.text = $"{_viewModel.Soul:N0}";
        }

        else if (propertyName == nameof(ResourceModel.Money))
        {
            _moneyText.text = $"{_viewModel.Money:N0}";
        }
    }
}
