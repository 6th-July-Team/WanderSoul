using UnityEngine;

public enum DamageType
{
    None,
    Physical,
    Fire,
    Cold,
    Electric,
    Magic,
}

public enum Grade
{
    None,
    Common,
    Epic,
    Unique,
    Legendary,
}

public enum SkillType
{
    None,
    Normal,
    Special,
}

public enum AttackType
{
    None,
    Melee,
    Ranged,
}

public enum StatType
{
    MaxHealth,
    BasicPower,
    AdditionalDamage,
    Defense,
    FireResistance,
    ColdResistance,
    ElectricResistance,
    ElementalResistance,
    MoveSpeed,
    HealthRegeneration,
    ManaRegeneration,
    CooldownReduction,
    BasicAttackCooldownReduction,
    SpecialAttackCooldownReduction,
    UltimateAttackCooldownReduction,
    MaxMana,
    MagicResistance,
    CritChance,
    CritMultiplier,
    LifeSteal,
    MaxReviveCount,
    MaxDashCount,
    MagnetRadius,
    DashChargeTime,
    COUNT
}

public struct StatModifier
{
    public StatType StatType { get; }
    public StatModifierOperation Operation { get; }
    public float Value { get; }

    public StatModifier(StatType statType, StatModifierOperation operation, float value)
    {
        StatType = statType;
        Operation = operation;
        Value = value;
    }
}

public struct SkillModifier
{
    public SkillModifierScope Scope { get; }
    public string SkillId { get; }
    public SkillSlot SkillSlot { get; }
    public SkillValueType ValueType { get; }
    public StatModifierOperation Operation { get; }
    public float Value { get; }

    private SkillModifier(SkillModifierScope scope, string skillId, SkillSlot skillSlot
        , SkillValueType valueType, StatModifierOperation operation, float value)
    {
        Scope = scope;

        SkillId = skillId;
        SkillSlot = skillSlot;

        ValueType = valueType;
        Operation = operation;
        Value = value;
    }

    public static SkillModifier ForSkill(string skillId, SkillValueType valueType, StatModifierOperation operation, float value)
    {
        return new SkillModifier(SkillModifierScope.Skill, skillId, default, valueType, operation, value);
    }

    public static SkillModifier ForSlot(SkillSlot skillSlot, SkillValueType valueType, StatModifierOperation operation, float value)
    {
        return new SkillModifier(SkillModifierScope.Slot, null, skillSlot, valueType, operation, value);
    }

    public static SkillModifier ForAll(SkillValueType valueType, StatModifierOperation operation, float value)
    {
        return new SkillModifier(SkillModifierScope.All, null, default, valueType, operation, value);
    }
}

public struct ModifierHandle
{
    public int Id { get; }

    public ModifierHandle(int id)
    {
        Id = id;
    }
}