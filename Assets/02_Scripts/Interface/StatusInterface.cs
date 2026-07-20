
public interface IStatusEffectReceiver
{
    StatusEffectController StatusEffects { get; }
    IStatModifierReceiver StatModifierReceiver { get; }
    ISkillModifierReceiver SkillModifierReceiver { get; }
}

public interface IStatReader
{
    float GetValue(StatType statType);
}

public interface IStatModifierReceiver : IStatReader
{
    ModifierHandle AddModifier(StatModifier modifier);
    void RemoveModifier(ModifierHandle handle);
}
public interface IStatusEffectExecution
{
    void OnApply();
    void OnTick();
    void OnStackChanged(int stack);
    void OnRemove();
}