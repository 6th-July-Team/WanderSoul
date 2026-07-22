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

        // TODO(이태영): 궁극기는 PlayerSkillMaker가 아직 슬롯에 등록하지 않아 빈 슬롯으로 둔다.
        // PlayerSkillViewModel에 Ultimate가 등록되면 classData.UltimateSkillIds에서 선택된 것으로 연결할 것.
        SetSlot(_ultimateSlot, SkillSlot.Ultimate, null, skillViewModel, playerViewModel);
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
