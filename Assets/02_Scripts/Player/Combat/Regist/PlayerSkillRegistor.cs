

public static class PlayerSkillRegistor
{
    public static void RegisterAll(PlayerSkillRegistry registry)
    {
        registry.Register("ScholarBasic"
            , (createInfo) => new ScholarElementalArrow
            (
                createInfo.PetPartyReader
            ));

        registry.Register("ScholarSpecial"
            , (createInfo) => new ScholarElementalExplosion
            (
                
            ));

        registry.Register("ScholarUltBarrier"
            , (createInfo) => new ScholarSummonBarrier
            (

            ));

        registry.Register("ScholarUltBerserkSoul"
            , (createInfo) => new ScholarBererkSoul
            (
                createInfo.StatusEffectMaker,
                createInfo.PetStatusEffectReceivers
            ));
    }
}



public struct PlayerSkillCreateInfo
{
    public IPetPartyReader PetPartyReader;
    public IStatusEffectReceiver[] PetStatusEffectReceivers;
    public StatusEffectMaker StatusEffectMaker;

    public PlayerSkillCreateInfo(IPetPartyReader petPartyReader, IStatusEffectReceiver[] petStatusEffectReceivers
        , StatusEffectMaker statusEffectMaker)
    {
        PetPartyReader = petPartyReader;
        PetStatusEffectReceivers = petStatusEffectReceivers;
        StatusEffectMaker = statusEffectMaker;
    }
}