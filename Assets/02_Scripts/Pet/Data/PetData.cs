
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

public enum PetCommand
{
    PlayerFollow,
    Aggressive,
    GuardWagon,
    COUNT
}

public struct PetSkillUseContext
{
    public Vector3 PetPos;
    public PetActiveSkillData PetActiveSkillData;

    public PetSkillUseContext(Vector3 petPos, PetActiveSkillData petActiveSkillData)
    {
        PetPos = petPos;
        PetActiveSkillData = petActiveSkillData;
    }
}

public struct PetSkillCreateInfo
{
    public StatusEffectMaker StatusEffectMaker;

    public IStatusEffectReceiver PlayerReceiver;
    public IHealable PlayerHealable;

    public IStatModifierReceiver PetModifierReceiver;

    public PetActiveSkillData PetSkillData;
    public PetPassiveSkillData PetPassiveSkillData;
    public StatusEffectData EffectData;

    private PetSkillCreateInfo(StatusEffectMaker statusEffectMaker
        , IStatusEffectReceiver playerReceiver, IHealable playerHealable
        , IStatModifierReceiver petModifierReceiver
        , PetActiveSkillData petSkillData, PetPassiveSkillData petPassiveSkillData, StatusEffectData effectData)
    {
        StatusEffectMaker = statusEffectMaker;

        PlayerReceiver = playerReceiver;
        PlayerHealable = playerHealable;

        PetModifierReceiver = petModifierReceiver;

        PetSkillData = petSkillData;
        PetPassiveSkillData = petPassiveSkillData;
        EffectData = effectData;
    }

    public static PetSkillCreateInfo CreateActiveSkillInfo(StatusEffectMaker statusEffectMaker
        , IStatusEffectReceiver playerReceiver, IHealable playerHealable
        , IStatModifierReceiver petModifierReceiver
        , PetActiveSkillData petActiveSkillData, StatusEffectData effectDatas)
    {
        return new PetSkillCreateInfo(statusEffectMaker, playerReceiver, playerHealable, petModifierReceiver, petActiveSkillData, null, effectDatas);
    }

    public static PetSkillCreateInfo CreatePassiveSkillInfo(StatusEffectMaker statusEffectMaker
        , IStatusEffectReceiver playerReceiver, IHealable playerHealable
        , IStatModifierReceiver petModifierReceiver
        , PetPassiveSkillData petPassiveSkillData, StatusEffectData effectDatas)
    {
        return new PetSkillCreateInfo(statusEffectMaker, playerReceiver, playerHealable, petModifierReceiver, null, petPassiveSkillData, effectDatas);
    }
}