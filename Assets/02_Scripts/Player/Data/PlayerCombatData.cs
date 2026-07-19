using UnityEngine;

public enum SkillSlot
{
    Basic,
    Special,
    Ultimate
}

public enum StatModifierOperation
{
    Flat,
    AddPercent,
    MultiplePercent
}

public struct PlayerSkillUseContext
{
    public Transform Owner { get; }
    public Vector3 AimDirection { get; }
    public Vector3 AimWorldPoint { get; }

    public PlayerSkillUseContext(Transform owner, Vector3 aimDirection, Vector3 aimWorldPoint)
    {
        Owner = owner;
        AimDirection = aimDirection;
        AimWorldPoint = aimWorldPoint;
    }
}