using Cysharp.Threading.Tasks;
using System;
using TMPro;
using UnityEngine;

public class SimplePopupUIView : BaseUI
{
    [SerializeField] private TMP_Text _messageText;
    [SerializeField] private UISlideAnimation _slideAnimation;
    [SerializeField] private float _showDuration = 2f;

    public void SetPopup(string message)
    {
        _messageText.text = message;

    }

    protected override void OnOpened()
    {
        AutoCloseAsync().Forget();
    }

    private async UniTaskVoid AutoCloseAsync()
    {
        if (_slideAnimation != null)
        {
            _slideAnimation.SetHidden();
            _slideAnimation.SlideIn();
        }

        await UniTask.Delay(TimeSpan.FromSeconds(_showDuration));

        if (_slideAnimation != null)
        {
            _slideAnimation.SlideOut(CloseSelf);
        }
        else
        {
            CloseSelf();
        }
    }

    private void CloseSelf()
    {
        GameManager.UI.CloseUI(UIType.SimplePopupUIView);
    }
}