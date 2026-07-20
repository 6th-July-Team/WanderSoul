using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.EventSystems;
using System.Net.NetworkInformation;

public class LevelUpOptionSlotUIView : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Image _backgroundImage;
    [SerializeField] private Image _iconImage;
    [SerializeField] private TMP_Text _nameText;
    [SerializeField] private TMP_Text _gradeText;
    [SerializeField] private TMP_Text _descriptionText;
    [SerializeField] private TMP_Text _statValueText;
    [SerializeField] private UIButton _selectButton;

    [Header("Grade Colors")]
    [SerializeField] private Color _commonColor;
    [SerializeField] private Color _rareColor;
    [SerializeField] private Color _epicColor;
    [SerializeField] private Color _legendaryColor;

    [Header("Animations")]
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private RectTransform _rootRect;
    [SerializeField] private float _hoverScale = 1.05f;
    [SerializeField] private float _hoverDuration = 0.15f; 

    private string _optionId;

    private event Action<string> OnSlotSelected;

    public string OptionId
    {
        get { return _optionId; }
    }

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

    public void InitSlot(string optionId)
    {

        if (string.IsNullOrEmpty(optionId) == true)
        {
            return;
        }

        _optionId = optionId;

        var optionData = GameManager.DataTable.GetLevelUpOptionData(optionId);

        if (optionData == null)
        {
            return;
        }

        _nameText.text = optionData.Name;
        _gradeText.text = optionData.stringGrade;
        _descriptionText.text = optionData.Description;
        _statValueText.text = GetStatValueText(optionData);

        RefreshBackgroundColor(optionData.stringGrade);
        RefreshIcon(optionData.IconPath);
    }

    private string GetStatValueText(LevelUpOptionData optionData)
    {
        string sign = string.Empty;
        string suffix = string.Empty;

        if (optionData.stringOperation == "Flat")
        {
            sign = "+";
        }
        else if (optionData.stringOperation == "AddPercent")
        {
            sign = "+";
            suffix = "%";
        }
        else if (optionData.stringOperation == "MultiplePercent")
        {
            sign = "x";
        }

        return $"{optionData.stringTargetStatType} {sign}{optionData.Value}{suffix}";
    }
    private void RefreshBackgroundColor(string grade)
    {
        _backgroundImage.color = GetGradeColor(grade);
    }

    private Color GetGradeColor(string grade)
    {
        if (grade == "Common")
        {
            return _commonColor;
        }
        else if (grade == "Rare")
        {
            return _rareColor;
        }
        else if (grade == "Epic")
        {
            return _epicColor;
        }
        else if (grade == "Legendary")
        {
            return _legendaryColor;
        }
        else
        {
            return Color.gray;
        }
    }

    private void RefreshIcon(string iconPath)
    {
        if (string.IsNullOrEmpty(iconPath) == true)
        {
            return;
        }

        Sprite iconSprite = Utils.ResourcesLoad<Sprite>(iconPath);

        if (iconSprite != null)
        {
            _iconImage.sprite = iconSprite;
        }
    }


    public void BindSelectEvent(Action<string> onSelected)
    {
        OnSlotSelected = onSelected;
    }
    private void OnClickSelect()
    {
        if (OnSlotSelected != null)
        {
            OnSlotSelected.Invoke(_optionId);
        }
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
