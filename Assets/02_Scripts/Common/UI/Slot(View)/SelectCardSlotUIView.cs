using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.EventSystems;

public abstract class SelectCardSlotUIView : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] protected Image _iconImage;
    [SerializeField] protected TMP_Text _nameText;
    [SerializeField] protected TMP_Text _descriptionText;
    [SerializeField] private UIButton _selectButton;

    [Header("Animations")]
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private RectTransform _rootRect;
    [SerializeField] private float _hoverScale = 1.05f;
    [SerializeField] private float _hoverDuration = 0.15f;

    protected string _slotId;

    private event Action<string> OnSlotSelected;

    public string SlotId
    {
        get { return _slotId; }
    }

    public abstract void InitSlot(string id);

    private void OnEnable()
    {
        _selectButton.BindOnClickButtonEvent(OnClickSelect);
    }

    private void OnDisable()
    {
        OnSlotSelected = null;
        _rootRect.DOKill();
        _canvasGroup.DOKill();
    }

    public void BindSelectEvent(Action<string> onSelected)
    {
        OnSlotSelected = onSelected;
    }

    private void OnClickSelect()
    {
        if (OnSlotSelected != null)
        {
            OnSlotSelected.Invoke(_slotId);
        }
    }

    protected void RefreshIcon(string iconPath)
    {
        if (_iconImage == null)
        {
            return;
        }

        if (string.IsNullOrEmpty(iconPath) == true)
        {
            _iconImage.gameObject.SetActive(false);
            return;
        }

        Sprite iconSprite = Utils.ResourcesLoad<Sprite>(iconPath);

        if (iconSprite == null)
        {
            _iconImage.gameObject.SetActive(false);
            return;
        }

        _iconImage.gameObject.SetActive(true);
        _iconImage.sprite = iconSprite;
    }

    public void PlayAppearAnimation(float delay)
    {
        _canvasGroup.alpha = 0f;
        _rootRect.localScale = Vector3.one * 0.8f;

        Sequence sequence = DOTween.Sequence();
        sequence.AppendInterval(delay);
        sequence.Append(_canvasGroup.DOFade(1f, 0.5f));
        sequence.Join(_rootRect.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack));
    }

    public void PlaySelectedAnimation(Action onComplete)
    {
        _rootRect.DOKill();

        Sequence sequence = DOTween.Sequence();
        sequence.Append(_rootRect.DOScale(Vector3.one * 1.2f, 0.2f).SetEase(Ease.OutBack));
        sequence.Append(_canvasGroup.DOFade(0f, 0.2f));
        sequence.OnComplete(() =>
        {
            if (onComplete != null)
            {
                onComplete.Invoke();
            }
        });
    }

    public void PlayUnselectedAnimation()
    {
        _rootRect.DOKill();
        _canvasGroup.DOFade(0f, 0.2f);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _rootRect.DOKill();
        _rootRect.DOScale(Vector3.one * _hoverScale, _hoverDuration).SetEase(Ease.OutQuad);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _rootRect.DOKill();
        _rootRect.DOScale(Vector3.one, _hoverDuration).SetEase(Ease.OutQuad);
    }
}
