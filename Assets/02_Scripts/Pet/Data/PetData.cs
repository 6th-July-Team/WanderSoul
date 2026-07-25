
using UnityEngine;

public enum PetElement
{
    None,
    Fire,
    Water,
    Earth,
    Air,
    Magic,
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
    public IPet IPet;
    public Vector3 PetPos;
    public PetActiveSkillData PetActiveSkillData;

    public PetSkillUseContext(IPet iPet, Vector3 petPos, PetActiveSkillData petActiveSkillData)
    {
        IPet = iPet;
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

    public IStatusEffectReceiver PetReceiver;
    public IPet IPet;

    private PetSkillCreateInfo(StatusEffectMaker statusEffectMaker
        , IStatusEffectReceiver playerReceiver, IHealable playerHealable
        , IStatModifierReceiver petModifierReceiver
        , PetActiveSkillData petSkillData, PetPassiveSkillData petPassiveSkillData, StatusEffectData effectData
        , IStatusEffectReceiver petReceiver, IPet iPet)
    {
        StatusEffectMaker = statusEffectMaker;

        PlayerReceiver = playerReceiver;
        PlayerHealable = playerHealable;

        PetModifierReceiver = petModifierReceiver;

        PetSkillData = petSkillData;
        PetPassiveSkillData = petPassiveSkillData;
        EffectData = effectData;

        PetReceiver = petReceiver;
        IPet = iPet;
    }

    public static PetSkillCreateInfo CreateActiveSkillInfo(StatusEffectMaker statusEffectMaker
        , IStatusEffectReceiver playerReceiver, IHealable playerHealable
        , IStatModifierReceiver petModifierReceiver
        , PetActiveSkillData petActiveSkillData, StatusEffectData effectDatas
        , IStatusEffectReceiver petReceiver)
    {
        return new PetSkillCreateInfo(statusEffectMaker, playerReceiver, playerHealable
            , petModifierReceiver, petActiveSkillData, null, effectDatas, petReceiver, null);
    }

    public static PetSkillCreateInfo CreatePassiveSkillInfo(StatusEffectMaker statusEffectMaker
        , IStatusEffectReceiver playerReceiver, IHealable playerHealable
        , IStatModifierReceiver petModifierReceiver
        , PetPassiveSkillData petPassiveSkillData, StatusEffectData effectDatas, IPet iPet)
    {
        return new PetSkillCreateInfo(statusEffectMaker, playerReceiver, playerHealable
            , petModifierReceiver, null, petPassiveSkillData, effectDatas, null, iPet);
    }
}