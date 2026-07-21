using TMPro;
using UnityEngine;

public class ResourceHudUIView : BaseUI<ResourceHudUIView, ResourceHudViewModel>
{
    [Header("Layout")]
    [SerializeField] private RectTransform _rootRect;
    [SerializeField] private Vector2 _villageAnchoredPosition;
    [SerializeField] private Vector2 _convoyAnchoredPosition;

    [SerializeField] private TMP_Text _soulText;
    [SerializeField] private TMP_Text _moneyText;

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
