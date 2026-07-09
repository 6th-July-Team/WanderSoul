using UnityEngine;

public abstract class BaseView<T> : MonoBehaviour where T : BaseViewModel
{
    protected T _viewModel;

    public void BindViewModel(T viewModel)
    {
        _viewModel = viewModel;
        _viewModel.OnPropertyChanged_View += OnPropertyChanged;
        _viewModel.PropertyChangedOnInit();
    }

    protected virtual void OnDestroy()
    {
        if (_viewModel != null)
        {
            _viewModel.OnPropertyChanged_View -= OnPropertyChanged;
            _viewModel.Dispose();
        }
    }

    protected abstract void OnPropertyChanged(string propertyName);
}
