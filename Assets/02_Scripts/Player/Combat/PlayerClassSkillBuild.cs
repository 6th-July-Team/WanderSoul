using System.Collections.Generic;
using UnityEngine;

public class PlayerClassSkillBuild
{
    // Skill
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

}
