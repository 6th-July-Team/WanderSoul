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
        SetSlot(_ultimateSlot, SkillSlot.Ultimate, GetEquippedUltimateSkillId(classData)
            , skillViewModel);
    }
    public void SetUltimateSkill(string skillId)
    {
        var skillViewModel = GameManager.Network.RequestPlayerSkillViewModel();

        SetSlot(_ultimateSlot, SkillSlot.Ultimate, skillId, skillViewModel);
    }

    // TODO(이태영): PlayerSkillMaker의 임시 궁극기 장착(UltimateSkillIds[0])이 정리되면 같이 맞추기
    private string GetEquippedUltimateSkillId(PlayerClassData classData)
    {
        if (classData.UltimateSkillIds == null || classData.UltimateSkillIds.Count == 0)
        {
            return null;
        }

        return classData.UltimateSkillIds[0];
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
