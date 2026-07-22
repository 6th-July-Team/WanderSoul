using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIButton : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private Button _button;
    [SerializeField] private TMP_Text _text;

    private void Awake()
    {
        InitButton();
    }

    private void OnDisable()
    {
        //if (_button != null)
        //{
        //    _button.onClick.RemoveAllListeners();
        //}
    }

    private void InitButton()
    {
        if (_button != null)
        {
            return;
        }

        Button button = GetComponentInChildren<Button>();
        if (button != null)
        {
            _button = button;
        }
    }

    public void BindOnClickButtonEvent(Action onClickCallback)
    {
        if (_button == null)
        {
            Debug.LogWarning($"{gameObject.name}: _button이 null!");
            return;
        }
        _button.onClick.RemoveAllListeners();
        _button.onClick.AddListener(new UnityEngine.Events.UnityAction(onClickCallback));
    }

    public void UnbindOnClickButtonEvent(Action onClickCallback)
    {
        if (_button == null)
        {
            return;
        }

        _button.onClick.RemoveListener(new UnityEngine.Events.UnityAction(onClickCallback));
    }

    public void ChangeText(string content)
    {
        if (_text == null)
        {
            return;
        }

        _text.text = content;
    }

    public void SetInteractable(bool isInteractable)
    {
        if (_button == null)
        {
            return;
        }

        _button.interactable = isInteractable;
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        // TODO(태영,07-08): 클릭 애니메이션/사운드 추가 예정
    }
}
