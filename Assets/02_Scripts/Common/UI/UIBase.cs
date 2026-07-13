using UnityEngine;

public abstract class BaseUI : MonoBehaviour
{
    public bool IsActive { get; private set; }

    private bool _isInitialized = false;

    public void Init()
    {
        if (_isInitialized == true)
        {
            return;
        }

        _isInitialized = true;
        OnInit();
    }

    public void ActiveTrue()
    {
        gameObject.SetActive(true);
        IsActive = true;
        OnOpened();
    }

    public void ActiveFalse()
    {
        OnClosed();
        IsActive = false;
        gameObject.SetActive(false);
    }

    protected virtual void OnInit()
    {
    }

    protected virtual void OnOpened()
    {
    }

    protected virtual void OnClosed()
    {
    }
}


public abstract class BaseUI<TView, TViewModel> : BaseUI where TView : BaseUI<TView, TViewModel> where TViewModel : BaseViewModel
{
    protected TViewModel _viewModel;

    public void BindViewModel(TViewModel viewModel)
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
//public abstract class BaseUI<T> : BaseUI where T : BaseUI<T>
//{
//}