using UnityEngine;

public class PetSkillExecutor
{
    private readonly PetSkillEffectHandlerRegistry _registry;

    public PetSkillExecutor(PetSkillEffectHandlerRegistry registry)
    {
        _registry = registry;
    }

    public void Execute(PetCombatRuntime runtime, PetCombatContext context)
    {
        PetSkillData skillData = runtime.SkillData;

        if (skillData.Effects == null || skillData.Effects.Count == 0)
            return;

        foreach (var effectData in skillData.Effects)
        {
            if (!_registry.TryGetHandler(effectData.EffectType, out var handler))
            {
                Debug.LogError($"등록되지 않은 스킬 효과입니다. EffectType: {effectData.EffectType}");
                continue;
            }

            if (!handler.CanApply(effectData, runtime, context))
                continue;

            handler.Apply(effectData, runtime, context);
        }
    }
}
