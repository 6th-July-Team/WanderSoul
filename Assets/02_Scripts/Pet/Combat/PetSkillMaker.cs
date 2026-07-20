
public class PetSkillMaker
{
    PetActiveSkillExecutionRegistry _activeRegistry;
    PetPassiveSkillExecutionRegistry _passiveRegistry;
    private StatusEffectMaker _statusEffectMaker;

    public PetSkillMaker(PetActiveSkillExecutionRegistry activeRegistry, PetPassiveSkillExecutionRegistry passiveRegistry
        , StatusEffectMaker statusEffectMaker)
    {
        _activeRegistry = activeRegistry;
        _passiveRegistry = passiveRegistry;
        _statusEffectMaker = statusEffectMaker;
    }

    public PetCombatController CreateCombatController(string id, PetStatController statController
        , IStatusEffectReceiver playerReceiver, IHealable playerHealable
        , IStatModifierReceiver petModifierReceiver)
    {
        PetCombatController combatController = new();

        PetData petData = GameManager.DataTable.GetPetData(id);


        foreach (var skillId in petData.ActiveSkillIds)
        {
            CreateActiveSkill(skillId, combatController, statController, playerReceiver, playerHealable, petModifierReceiver);
        }

        foreach (var skillId in petData.PassiveSkillIds)
        {
            CreatePassiveSkill(skillId, combatController, playerReceiver, playerHealable, petModifierReceiver);
        }

        return combatController;
    }

    private void CreateActiveSkill(string skillId, PetCombatController combatController
        , PetStatController statController, IStatusEffectReceiver playerReceiver, IHealable playerHealable
        , IStatModifierReceiver petModifierReceiver)
    {
        PetActiveSkillData skillData = GameManager.DataTable.GetPetActiveSkillData(skillId);

        StatusEffectData effectData = null;

        if (skillData.StatusEffectId != null)
        {
            string StatusEffectId = skillData.StatusEffectId;
            effectData= GameManager.DataTable.GetStatusEffectData(StatusEffectId);
        }

        PetSkillCreateInfo createInfo 
            = PetSkillCreateInfo.CreateActiveSkillInfo(_statusEffectMaker, playerReceiver, playerHealable
                                                        , petModifierReceiver, skillData, effectData);

        IPetActiveSkillExecution execution = _activeRegistry.Create(skillData.ExecutionId, createInfo);

        if (null == execution)
            return;

        PetActiveSkill activeSkill = new PetActiveSkill(skillData, execution, statController);

        combatController.SetActiveSkill(skillData.GetSkillType(), activeSkill);
    }

    private void CreatePassiveSkill(string skillId, PetCombatController combatController
        , IStatusEffectReceiver playerReceiver, IHealable playerHealable
        , IStatModifierReceiver petModifierReceiver)
    {
        PetPassiveSkillData skillData = GameManager.DataTable.GetPetPassiveSkillData(skillId);

        StatusEffectData effectData = null;

        if (skillData.StatusEffectId != null)
        {
            string StatusEffectId = skillData.StatusEffectId;
            effectData = GameManager.DataTable.GetStatusEffectData(StatusEffectId);
        }

        PetSkillCreateInfo createInfo 
            = PetSkillCreateInfo.CreatePassiveSkillInfo(_statusEffectMaker, playerReceiver, playerHealable
                                                        , petModifierReceiver, skillData, effectData);

        IPetPassiveSkillExecution execution = _passiveRegistry.Create(skillData.ExecutionId, createInfo);

        if (null == execution)
            return;

        PetPassiveSkill passiveSkill = new PetPassiveSkill(skillData, execution);

        combatController.AddPassiveSkill(passiveSkill);
    }
}
