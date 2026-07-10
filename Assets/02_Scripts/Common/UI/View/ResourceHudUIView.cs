using TMPro;
using UnityEngine;

public class ResourceHudUIView : BaseUI<ResourceHudUIView, ResourceHudViewModel>
{
    [SerializeField] private TMP_Text _soulText;
    [SerializeField] private TMP_Text _moneyText;

    protected override void OnPropertyChanged(string propertyName)
    {
        //NO는 숫자 표시 형식
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
