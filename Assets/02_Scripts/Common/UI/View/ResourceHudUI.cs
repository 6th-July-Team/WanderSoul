using TMPro;
using UnityEngine;

public class ResourceHudUI : BaseUI<ResourceHudUI>
{
    [SerializeField] private TMP_Text _soulText;
    [SerializeField] private TMP_Text _moneyText;


    private ResourceHudViewModel _viewModel;

    public void BindViewModel(ResourceHudViewModel viewModel)
    {

        if (_viewModel != null)
        {
            _viewModel.OnPropertyChanged_View -= OnPropChanged_View;
        }

        _viewModel = viewModel;
        _viewModel.OnPropertyChanged_View += OnPropChanged_View;

        _viewModel.PropertyChangedOnInit();
    }

    private void OnDestroy()
    {
        if (_viewModel != null)
        {
            _viewModel.OnPropertyChanged_View -= OnPropChanged_View;
            _viewModel.Dispose();
        }
    }

    private void OnPropChanged_View(string propertyName)
    {
        //NO는 숫자 표시 형식
        if (propertyName == nameof(ResourceModel.Soul))
        {
            _soulText.text = $"{_viewModel.Soul:NO}";
        }

        else if (propertyName == nameof(ResourceModel.Money))
        {
            _soulText.text = $"{_viewModel.Money:NO}";
        }
    }
}
