using System.Collections.Generic;

public class PetCombatController
{
    private readonly Dictionary<SkillType, PetActiveSkill> _aciveSkills = new();
    private readonly List<PetPassiveSkill> _passiveSkills = new();


    public void Update(float deltaTime)
    {
        foreach (PetActiveSkill aciveSkill in _aciveSkills.Values)
        {
            if (aciveSkill != null)
                aciveSkill.Update(deltaTime);
        }

        foreach (PetPassiveSkill passiveSkill in _passiveSkills)
        {
            if (passiveSkill != null)
                passiveSkill.Update(deltaTime);
        }
    }

    public PetActiveSkill SelectSkill(ITargetable target)
    {
        foreach (PetActiveSkill skill in _aciveSkills.Values)
        {
            if (!skill.IsReady || !skill.CanUse(target))
                continue;

            return skill;
        }

        return null;
    }

    public bool TryExecute(PetActiveSkill skill, PetSkillUseContext context)
    {
        if (skill == null || !skill.IsReady)
            return false;

        bool started = skill.TryExecute(context);


        return started;
    }

    public void SetActiveSkill(SkillType skillType, PetActiveSkill petSkill)
    {
        if (petSkill == null)
        {
            _aciveSkills.Remove(skillType);
            return;
        }

        _aciveSkills[skillType] = petSkill;
    }

    public void AddPassiveSkill(PetPassiveSkill skill)
    {
        if (skill == null)
            return;

        _passiveSkills.Add(skill);
        skill.Activate();
    }

    public void Release()
    {
        foreach (PetPassiveSkill passiveSkill in _passiveSkills)
            passiveSkill.Deactivate();

        _passiveSkills.Clear();
        _aciveSkills.Clear();
    }

    public PetActiveSkillData GetSkillInfo(SkillType skillType)
    {
        return _aciveSkills.TryGetValue(skillType, out PetActiveSkill skill) ? skill.SkillData : null;
    }
}