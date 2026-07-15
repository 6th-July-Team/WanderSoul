using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelUpOptionSlotUIView : MonoBehaviour
{
    [SerializeField] private Image _backgroundImage;
    [SerializeField] private Image _iconImage;
    [SerializeField] private TMP_Text _nameText;
    [SerializeField] private TMP_Text _gradeText;
    [SerializeField] private TMP_Text _descriptionText;
    [SerializeField] private UIButton _selectButton;

    [Header("Grade Colors")]
    [SerializeField] private Color _commonColor;
    [SerializeField] private Color _rareColor;
    [SerializeField] private Color _epicColor;
    [SerializeField] private Color _legendaryColor;

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
        _gradeText.text = optionData.Grade;
        _descriptionText.text = optionData.Description;

        RefreshBackgroundColor(optionData.Grade);
        RefreshIcon(optionData.IconPath);
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

}
