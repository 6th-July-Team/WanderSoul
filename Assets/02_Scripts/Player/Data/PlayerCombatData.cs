using System.Collections.Generic;
using UnityEngine;

public enum SkillSlot
{
    Basic,
    Special,
    Ultimate
}

public enum PlayerStatType
{
    MaxHealth,
    BasicAttackPower,
    Defense,
    FireResistance,
    ColdResistance,
    ElectricResistance,
    ElementalResistance,
    MoveSpeed,
    HealthRegeneration,
    ManaRegeneration,
    CooldownReduction,
    MaxMana,
    COUNT
}

public enum StatModifierOperation
{
    Flat,
    AddPercent,
    MultiplePercent
}


public struct SkillUseContext
{
    public Transform Owner { get; }
    public Vector3 AimDirection { get; }
    public Vector3 AimWorldPoint { get; }

    public SkillUseContext(Transform owner, Vector3 aimDirection, Vector3 aimWorldPoint)
    {
        Owner = owner;
        AimDirection = aimDirection;
        AimWorldPoint = aimWorldPoint;
    }
}

public struct StatModifier
{
    public PlayerStatType StatType { get; }
    public StatModifierOperation Operation { get; }
    public float Value { get; }

    public StatModifier(PlayerStatType statType, StatModifierOperation operation, float value)
    {
        StatType = statType;
        Operation = operation;
        Value = value;
    }
}