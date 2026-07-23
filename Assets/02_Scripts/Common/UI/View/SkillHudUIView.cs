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
        var playerViewModel = GameManager.Network.RequestCreatePlayer();

        SetSlot(_basicSlot, SkillSlot.Basic, classData.BasicSkillId, skillViewModel, playerViewModel);
        SetSlot(_specialSlot, SkillSlot.Special, classData.SpecialSkillId, skillViewModel, playerViewModel);
        SetSlot(_ultimateSlot, SkillSlot.Ultimate, GetEquippedUltimateSkillId(classData)
            , skillViewModel, playerViewModel);
    }
    public void SetUltimateSkill(string skillId)
    {
        var skillViewModel = GameManager.Network.RequestPlayerSkillViewModel();
        var playerViewModel = GameManager.Network.RequestCreatePlayer();

        SetSlot(_ultimateSlot, SkillSlot.Ultimate, skillId, skillViewModel, playerViewModel);
    }

    private string GetEquippedUltimateSkillId(PlayerClassData classData)
    {
        if (classData.UltimateSkillIds == null || classData.UltimateSkillIds.Count == 0)
        {
            return null;
        }

        if (classData.UltimateSkillIds.Count > 1)
        {
            return classData.UltimateSkillIds[1];
        }

        return classData.UltimateSkillIds[0];
    }

    private void SetSlot(SkillSlotUiView slotView, SkillSlot slot, string skillId
        , PlayerSkillViewModel skillViewModel, PlayerViewModel playerViewModel)
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

        slotView.SetSkill(slot, skillData, skillViewModel, playerViewModel);
    }
}
