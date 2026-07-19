using UnityEngine;

public class StatusEffectMaker
{
    private StatusEffectRegistry _registry;

    public StatusEffectMaker(StatusEffectRegistry registry)
    {
        _registry = registry;
    }

    public StatusEffectInstance Create(IStatusEffectReceiver receiver, StatusEffectData data
        , SkillModifierData skillModifierData = null)
    {
        if (receiver == null || data == null)
            return null;

        StatusEffectCreateInfo createInfo = new StatusEffectCreateInfo(receiver, data, skillModifierData);

        IStatusEffectExecution execution = _registry.CreateExecution(data.ExecutionId, createInfo);

        if(null == execution)
        {
            Debug.LogError($"Failed to create execution for StatusEffect with ExecutionId: {data.ExecutionId}");
            return null;
        }

        return new StatusEffectInstance(data, execution);
    }
}
