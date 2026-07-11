
public interface IPetSkillable
{
    bool CanExecuteSkill();
    void Execute();
}

public interface IPetSkillEffectHandler
{
    EPetSkillEffectType EffectType { get; }

    bool CanApply(PetSkillEffectData effectData, PetCombatRuntime runtime, PetCombatContext context);

    void Apply(PetSkillEffectData effectData, PetCombatRuntime runtime, PetCombatContext context);
}