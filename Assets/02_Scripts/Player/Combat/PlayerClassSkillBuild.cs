using System.Collections.Generic;

public class PlayerClassSkillBuild
{
    private Dictionary<SkillSlot, PlayerSkill> _skills = new();

    public PlayerClassSkillBuild(PlayerSkill basicSkill, PlayerSkill specialSkill, PlayerSkill ultimateSkill)
    {
        _skills[SkillSlot.Basic] = basicSkill;
        _skills[SkillSlot.Special] = specialSkill;
        _skills[SkillSlot.Ultimate] = ultimateSkill;
    }

    public void Update(float deltaTime)
    {
        foreach (var skill in _skills.Values)
        {
            if(skill != null)
                skill.Update(deltaTime);
        }
    }

    public void CheckSkillRange(SkillSlot skillSlot, PlayerSkillUseContext context)
    {
        if (!_skills.TryGetValue(skillSlot, out PlayerSkill skill))
            return;

        skill?.CheckSkillRange(context);
    }

    public void HideSkillRange(SkillSlot skillSlot)
    {
        if (!_skills.TryGetValue(skillSlot, out PlayerSkill skill))
            return;

        skill?.HideSkillRange();
    }

    public bool TryExecuteSkill(SkillSlot skillSlot, PlayerSkillUseContext context)
    {
        if (!_skills.TryGetValue(skillSlot, out PlayerSkill skill))
            return false;

        return skill.TryExecuteSkill(context);
    }

    public void SetSkill(SkillSlot slot, PlayerSkill skill)
    {
        if (skill == null)
        {
            _skills.Remove(slot);
            return;
        }

        _skills[slot] = skill;
    }

    public PlayerSkillData GetSkillInfo(SkillSlot slot)
    {
        return _skills.TryGetValue(slot, out PlayerSkill skill) ? skill.SkillData : null;
    }

    public float GetSkillCoolTime(SkillSlot slot)
    {
        return _skills[slot].RemainingCooldTime;
    }

}
