using System;
using UnityEngine;

public enum PetElement
{
    None,
    Fire,
    Water,
    Earth,
    Air,
    COUNT
}

public enum PetRole
{
    Attacker,
    Defender,
    Support,
    COUNT
}

public enum PetSkillType
{
    NormalAttack,
    Special,
    COUNT
}

public enum PetSkillTrigger
{
    AutoAttack,
    AutoInterval,
    Passive,
    COUNT
}

public enum SkillBehavior
{
    MeleeConeDamage,
    ProjectileAttack,
    AreaHeal,
    Shield,
    BuffAura,
    Taunt,
    COUNT
}

public enum PetCommand
{
    PlayerFollow,
    Aggressive,
    GuardWagon,
    COUNT
}

[Serializable]
public class ResistanceInfo
{
    public float FireResistance;
    public float WaterResistance;
    public float EarthResistance;
    public float AirResistance;
}



public readonly struct PetCombatContext
{
    public readonly PetController Pet;
    public readonly ITargetable Target;
    public readonly Transform Player;
    public readonly Transform Cart;

    public readonly LayerMask TargetLayerMask;
    public readonly Collider[] SearchBuffer;

    public PetCombatContext(
        PetController pet,
        ITargetable target,
        Transform player,
        Transform cart,
        LayerMask targetLayerMask,
        Collider[] searchBuffer)
    {
        Pet = pet;
        Target = target;
        Player = player;
        Cart = cart;
        TargetLayerMask = targetLayerMask;
        SearchBuffer = searchBuffer;
    }

    public PetCombatContext WithTarget(ITargetable target)
    {
        return new PetCombatContext(
            Pet,
            target,
            Player,
            Cart,
            TargetLayerMask,
            SearchBuffer
        );
    }
}

public enum EDamageType
{
    None,
    Physical,
    Fire,
    Cold,
    Electric,
    Magic,
    TrueDamage
}