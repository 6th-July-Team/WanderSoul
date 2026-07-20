
public enum StatusEffectStackPolicy
{
    Ignore,
    RefreshDuration,
    AddStack,
    Independent,
    Replace
}

public struct StatusEffectCreateInfo
{
    public IStatusEffectReceiver Receiver { get; }
    public StatusEffectData EffectData { get; }
    public SkillModifierData SkillModifierData { get; }

    public StatusEffectCreateInfo(IStatusEffectReceiver receiver, StatusEffectData effectData, SkillModifierData skillModifierData)
    {
        Receiver = receiver;
        SkillModifierData = skillModifierData;
        EffectData = effectData;
    }
}