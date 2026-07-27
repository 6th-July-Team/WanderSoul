using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelUpOptionSlotUIView : SelectCardSlotUIView
{
    [Header("LevelUp Option")]
    [SerializeField] private Image _backgroundImage;
    [SerializeField] private TMP_Text _gradeText;
    [SerializeField] private TMP_Text _statValueText;
    [SerializeField] private LevelUpOptionSparkleUIView _sparkleEffect;

    [Header("Grade Colors")]
    [SerializeField] private Color _commonColor;
    [SerializeField] private Color _rareColor;
    [SerializeField] private Color _epicColor;
    [SerializeField] private Color _legendaryColor;

    public override void InitSlot(string optionId)
    {
        if (string.IsNullOrEmpty(optionId) == true)
        {
            return;
        }

        _slotId = optionId;

        var optionData = GameManager.DataTable.GetLevelUpOptionData(optionId);

        if (optionData == null)
        {
            Debug.LogWarning($"레벨업 옵션 데이터를 찾을 수 없습니다: {optionId}");
            return;
        }

        _nameText.text = optionData.Name;
        _descriptionText.text = optionData.Description;

        RefreshGrade(optionData.Grade);
        RefreshStatValue(optionData);
        RefreshIcon(optionData.IconPath);
    }

    private void RefreshStatValue(LevelUpOptionData optionData)
    {
        string statValueText = GetStatValueText(optionData);

        if (string.IsNullOrEmpty(statValueText) == true)
        {
            _statValueText.gameObject.SetActive(false);
            return;
        }

        _statValueText.gameObject.SetActive(true);
        _statValueText.text = statValueText;
    }

    private string GetStatValueText(LevelUpOptionData optionData)
    {
        if (string.IsNullOrEmpty(optionData.Operation) == true)
        {
            return string.Empty;
        }

        if (string.IsNullOrEmpty(optionData.TargetStatType) == true)
        {
            return string.Empty;
        }

        string statLabel = GetStatTypeLabel(optionData.TargetStatType);

        if (optionData.Operation == "AddPercent")
        {
            return $"{statLabel} +{optionData.Value * 100f:0.##}%";
        }
        else if (optionData.Operation == "MultiplePercent")
        {
            return $"{statLabel} x{optionData.Value:0.##}";
        }
        else
        {
            return $"{statLabel} +{optionData.Value:0.##}";
        }
    }

    private string GetStatTypeLabel(string statType)
    {
        if (statType == "MaxHealth")
        {
            return "최대 체력";
        }
        else if (statType == "AdditionalDamage")
        {
            return "추가 피해";
        }
        else if (statType == "MoveSpeed")
        {
            return "이동 속도";
        }
        else if (statType == "HealthRegeneration")
        {
            return "체력 재생";
        }
        else if (statType == "ManaRegeneration")
        {
            return "마나 재생";
        }
        else if (statType == "MagnetRadius")
        {
            return "흡수 반경";
        }
        else if (statType == "MaxDashCount")
        {
            return "대쉬 횟수";
        }
        else if (statType == "MaxReviveCount")
        {
            return "부활 횟수";
        }
        else
        {
            return statType;
        }
    }

    private void RefreshGrade(string grade)
    {
        Color gradeColor = GetGradeColor(grade);

        _backgroundImage.color = gradeColor;

        if (_gradeText != null)
        {
            _gradeText.color = gradeColor;
            _gradeText.text = GetGradeLabel(grade);
        }

        if (_sparkleEffect != null)
        {
            _sparkleEffect.ApplyGrade(grade, gradeColor);
        }
    }

    private string GetGradeLabel(string grade)
    {
        if (grade == "Common")
        {
            return "일반";
        }
        else if (grade == "Rare")
        {
            return "희귀";
        }
        else if (grade == "Epic")
        {
            return "영웅";
        }
        else if (grade == "Legendary")
        {
            return "전설";
        }
        else
        {
            return grade;
        }
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
}
