using UnityEngine;

public class SkillHudUIView : BaseUI
{
    [SerializeField] private SkillSlotUiView _basicSlot;
    [SerializeField] private SkillSlotUiView _specialSlot;
    [SerializeField] private SkillSlotUiView _ultimateSlot;

    public void SetSkills(PlayerCombatController combatComtroller)
    {

        if (combatComtroller == null)
        {
            return;
        }

        var manaPool = combatComtroller.ManaPool;

        // TODO: GetSkill() 메서드 추가되면 주석 풀기
        // _basicSlot.SetSkill(combatController.GetSkill(SkillSlot.Basic), manaPool);
        // _specialSlot.SetSkill(combatController.GetSkill(SkillSlot.Special), manaPool);
        // _ultimateSlot.SetSkill(combatController.GetSkill(SkillSlot.Ultimate), manaPool);
    }
}
