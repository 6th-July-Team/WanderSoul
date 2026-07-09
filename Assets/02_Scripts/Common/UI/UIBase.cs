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

public abstract class BaseUI<T> : BaseUI where T : BaseUI<T>
{
}