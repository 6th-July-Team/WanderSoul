using TMPro;
using UnityEngine;

public class UltimateSkillSlotUIView : SelectCardSlotUIView
{
    [Header("Ultimate Skill")]
    [SerializeField] private TMP_Text _costText;

    public override void InitSlot(string skillId)
    {
        if (string.IsNullOrEmpty(skillId) == true)
        {
            return;
        }

        _slotId = skillId;

        var skillData = GameManager.DataTable.GetPlayerSkillData(skillId);

        if (skillData == null)
        {
            Debug.LogWarning($"스킬 데이터를 찾을 수 없습니다: {skillId}");
            return;
        }

        _nameText.text = skillData.Name;
        _descriptionText.text = skillData.Description;

        RefreshCost(skillData);
        RefreshIcon(skillData.IconPath);
    }

    private void RefreshCost(PlayerSkillData skillData)
    {
        if (_costText == null)
        {
            return;
        }

        string costText = GetCostText(skillData);

        if (string.IsNullOrEmpty(costText) == true)
        {
            _costText.gameObject.SetActive(false);
            return;
        }

        _costText.gameObject.SetActive(true);
        _costText.text = costText;
    }

    private string GetCostText(PlayerSkillData skillData)
    {
        bool hasCooldown = (skillData.Cooldown > 0f);
        bool hasManaCost = (skillData.ManaCost > 0);

        if (hasCooldown == true && hasManaCost == true)
        {
            return $"쿨타임 {skillData.Cooldown:0.##}초 \n마나 {skillData.ManaCost}";
        }
        else if (hasCooldown == true)
        {
            return $"쿨타임 {skillData.Cooldown:0.##}초";
        }
        else if (hasManaCost == true)
        {
            return $"마나 {skillData.ManaCost}";
        }
        else
        {
            return string.Empty;
        }
    }
}
