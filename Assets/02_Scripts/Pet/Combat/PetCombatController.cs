using System.Collections.Generic;

public class PetCombatController
{
    private readonly Dictionary<PetSkillSlot, PetActiveSkill> _aciveSkills = new();
    private readonly List<PetPassiveSkill> _passiveSkills = new();

    public bool IsBusy { get; private set; }

    public PetCombatController(PetActiveSkill basicSkill, PetActiveSkill specialSkill, List<PetPassiveSkill> passiveSkills)
    {
        _aciveSkills[PetSkillSlot.Normal] = basicSkill;
        _aciveSkills[PetSkillSlot.Special] = specialSkill;
        _passiveSkills = passiveSkills;
    }

    public void Update(float deltaTime)
    {
        foreach (var aciveSkill in _aciveSkills.Values)
        {
            if (aciveSkill != null)
                aciveSkill.Update(deltaTime);
        }
    }

    public PetActiveSkill SelectSkill(ITargetable target)
    {
        for (PetSkillSlot i = PetSkillSlot.COUNT - 1; i >= 0; i--)
        {
            if (_aciveSkills[i] != null && _aciveSkills[i].IsReady && _aciveSkills[i].CanTarget(target))
            {
                return _aciveSkills[i];
            }
        }

        return null;
    }

    public bool TryExecute(PetActiveSkill skill, PetSkillUseContext context)
    {
        if (IsBusy || skill == null || !skill.IsReady)
            return false;

        IsBusy = true;

        bool started = skill.TryExecute(context, OnEndSkill);

        if (!started)
            IsBusy = false;

        return started;
    }

    public void SetSkill(PetSkillSlot slot, PetActiveSkill petSkill)
    {
        if (petSkill == null)
        {
            _aciveSkills.Remove(slot);
            return;
        }

        _aciveSkills[slot] = petSkill;
    }

    public SOPetSkillInfo GetSkillInfo(PetSkillSlot slot)
    {
        return _aciveSkills.TryGetValue(slot, out PetActiveSkill skill) ? skill.Definition : null;
    }

    private void OnEndSkill()
    {
        IsBusy = false;
    }
}