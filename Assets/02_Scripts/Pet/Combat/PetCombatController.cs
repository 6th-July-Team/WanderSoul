using System.Collections.Generic;

public class PetCombatController
{
    private readonly Dictionary<SkillType, PetActiveSkill> _aciveSkills = new();
    private readonly List<PetPassiveSkill> _passiveSkills = new();

    //public bool IsBusy { get; private set; }

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
        if (/*IsBusy ||*/ skill == null || !skill.IsReady)
            return false;

        //IsBusy = true;

        bool started = skill.TryExecute(context/*, OnEndSkill*/);

        //if (!started)
        //    IsBusy = false;

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

        //IsBusy = false;
    }

    public PetActiveSkillData GetSkillInfo(SkillType skillType)
    {
        return _aciveSkills.TryGetValue(skillType, out PetActiveSkill skill) ? skill.SkillData : null;
    }

    // TODO: 패시브 스킬 데이터도 반환해야 하는데, 어떻게 엑티브와 동시에 반환 못하나?

    //private void OnEndSkill()
    //{
    //    IsBusy = false;
    //}
}