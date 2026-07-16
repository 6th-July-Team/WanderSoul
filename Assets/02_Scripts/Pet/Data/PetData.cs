
public enum PetElement
{
    None,
    Fire,
    Water,
    Earth,
    Air,
    COUNT
}

public enum PetSkillSlot
{
    Normal,
    Special,
    COUNT
}

public enum PetCommand
{
    PlayerFollow,
    Aggressive,
    GuardWagon,
    COUNT
}

public struct PetSkillUseContext
{

}

public enum PetStatType
{
    MaxHealth,
    BasicPower,
    Defense,
    FireResistance,
    ColdResistance,
    ElectricResistance,
    MagicResistance,
    MoveSpeed,
    HealthRegeneration,
    CooldownReduction,
    CritChance,
    CritMultiplier,
    LifeSteal,
    COUNT
}

public struct PetStatModifier
{
    public PetStatType StatType { get; }
    public StatModifierOperation Operation { get; }
    public float Value { get; }

    public PetStatModifier(PetStatType statType, StatModifierOperation operation, float value)
    {
        StatType = statType;
        Operation = operation;
        Value = value;
    }
}