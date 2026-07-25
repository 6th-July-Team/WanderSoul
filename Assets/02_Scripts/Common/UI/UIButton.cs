using DG.Tweening;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIButton : MonoBehaviour, IPointerClickHandler
    , IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private Button _button;
    [SerializeField] private TMP_Text _text;

    [Header("Animation")]
    [SerializeField] private RectTransform _scaleTarget;
    [SerializeField] private float _hoverScale = 1.08f;
    [SerializeField] private float _pressScale = 0.92f;
    [SerializeField] private float _duration = 0.12f;

    private Vector3 _originScale = Vector3.one;
    private bool _isHovering = false;

    private void Awake()
    {
        InitButton();
        InitScaleTarget();
    }

    private void OnEnable()
    {
        _isHovering = false;
        ResetScale();
    }

    private void InitScaleTarget()
    {
        if (_scaleTarget == null)
        {
            _scaleTarget = transform as RectTransform;
        }

        if (_scaleTarget != null)
        {
            _originScale = _scaleTarget.localScale;
        }
    }

    private void OnDisable()
    {
        _isHovering = false;
        ResetScale();

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
        // TODO(태영,07-08): 클릭 사운드 추가 예정
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (IsAnimatable() == false)
        {
            return;
        }

        _isHovering = true;
        ScaleTo(_hoverScale, Ease.OutQuad);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (IsAnimatable() == false)
        {
            return;
        }

        _isHovering = false;
        ScaleTo(1f, Ease.OutQuad);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (IsAnimatable() == false)
        {
            return;
        }

        ScaleTo(_pressScale, Ease.OutQuad);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (IsAnimatable() == false)
        {
            return;
        }

        float target = 1f;

        if (_isHovering == true)
        {
            target = _hoverScale;
        }

        ScaleTo(target, Ease.OutBack);
    }

    private bool IsAnimatable()
    {
        if (_scaleTarget == null)
        {
            return false;
        }

        if (_button != null && _button.interactable == false)
        {
            return false;
        }

        return true;
    }

    private void ScaleTo(float ratio, Ease ease)
    {
        _scaleTarget.DOKill();

        _scaleTarget.DOScale(_originScale * ratio, _duration).SetEase(ease);
    }

    private void ResetScale()
    {
        if (_scaleTarget == null)
        {
            return;
        }

        _scaleTarget.DOKill();
        _scaleTarget.localScale = _originScale;
    }
}
