

public static class PlayerSkillRegistor
{
    public static void RegisterAll(PlayerSkillRegistry registry)
    {
        registry.Register("ScholarBasic"
            , (createInfo) => new ScholarElementalArrow
            (
                createInfo.PetPartyReader
            ));

        registry.Register("ScholarBasic"
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

            ));
    }
}



public struct PlayerSkillCreateInfo
{
    public IPetPartyReader PetPartyReader;

    public PlayerSkillCreateInfo(IPetPartyReader petPartyReader)
    {
        PetPartyReader = petPartyReader;
    }
}