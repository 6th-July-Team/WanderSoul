using UnityEngine;

public class SkillHudUIView : BaseUI
{
    [SerializeField] private SkillSlotUiView _basicSlot;
    [SerializeField] private SkillSlotUiView _specialSlot;
    [SerializeField] private SkillSlotUiView _ultimateSlot;

    public void SetSkills(string playerClassId)
    {
        var classData = GameManager.DataTable.GetPlayerClassData(playerClassId);

        if (classData == null)
        {
            Debug.LogWarning($"플레이어 클래스 데이터를 찾을 수 없습니다: {playerClassId}");
            return;
        }

        var skillViewModel = GameManager.Network.RequestPlayerSkillViewModel();

        SetSlot(_basicSlot, SkillSlot.Basic, classData.BasicSkillId, skillViewModel);
        SetSlot(_specialSlot, SkillSlot.Special, classData.SpecialSkillId, skillViewModel);

        // 궁극기는 해금 후 SetUltimateSkill로 채워진다
        SetSlot(_ultimateSlot, SkillSlot.Ultimate, GetEquippedUltimateSkillId(), skillViewModel);
    }
    public void SetUltimateSkill(string skillId)
    {
        var skillViewModel = GameManager.Network.RequestPlayerSkillViewModel();

        SetSlot(_ultimateSlot, SkillSlot.Ultimate, skillId, skillViewModel);
    }

    private string GetEquippedUltimateSkillId()
    {
        return GameManager.Network.RequestPlayerOutGameViewModel().GetSelectedUltimateSkillId;
    }

    private void SetSlot(SkillSlotUiView slotView, SkillSlot slot, string skillId
        , PlayerSkillViewModel skillViewModel)
    {
        if (slotView == null)
        {
            return;
        }

        PlayerSkillData skillData = null;

        if (string.IsNullOrEmpty(skillId) == false)
        {
            skillData = GameManager.DataTable.GetPlayerSkillData(skillId);

            if (skillData == null)
            {
                Debug.LogWarning($"스킬 데이터를 찾을 수 없습니다: {skillId}");
            }
        }

        slotView.SetSkill(slot, skillData, skillViewModel);
    }
}
