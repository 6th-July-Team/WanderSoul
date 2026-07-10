using UnityEngine;

public interface IPetSkillable
{
    bool CanExecuteSkill();
    void Execute();
}

// 제거
//public interface IPetCommandState
//{
//    PetCommandResult GetCommandResult(SOPetSearch __SOPetSearch);
//}

//public interface IPetCommandState
//{
//    PetCommandResult Evaluate(PetCommandContext context);
//}

public interface IPetSkillEffectHandler
{
    EPetSkillEffectType EffectType { get; }

    bool CanApply(PetSkillEffectData effectData, PetCombatRuntime runtime, PetCombatContext context);

    void Apply(PetSkillEffectData effectData, PetCombatRuntime runtime, PetCombatContext context);
}