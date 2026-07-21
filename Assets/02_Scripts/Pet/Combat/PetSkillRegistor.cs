

public static class PetSkillRegistor
{
    public static void RegisterAllActiveSkills(PetActiveSkillExecutionRegistry registry)
    {

        registry.Register("WindBlessing"
            , (createInfo) => new PetWindBlessingSkill
            (
                createInfo.StatusEffectMaker,
                createInfo.EffectData,
                createInfo.PlayerReceiver,
                createInfo.PlayerHealable
            ));

        registry.Register("Aggro"
            , (createInfo) => new PetAggroSkill
            (
                createInfo.StatusEffectMaker,
                createInfo.PlayerReceiver,
                createInfo.PetReceiver
            ));

        registry.Register("Projectile", (createInfo) => new PetProjectileSkill());

        //registry.Register("Melee"
        //    , (createInfo) => new PetAggroSkill
        //    (
        //        createInfo.StatusEffectMaker,
        //        createInfo.PlayerReceiver,
        //        createInfo.PetReceiver
        //    ));
    }

    public static void RegisterAllPassiveSkills(PetPassiveSkillExecutionRegistry registry)
    {
        registry.Register("BuffPassive"
            , (createInfo) => new PetBuffPassive
            (
                createInfo.EffectData,
                createInfo.PetPassiveSkillData,
                createInfo.PlayerReceiver.StatModifierReceiver,
                createInfo.PetModifierReceiver
            ));

        registry.Register("AreaBuff"
           , (createInfo) => new PetAreaBuffPassive
           (
               createInfo.EffectData,
               createInfo.PetPassiveSkillData,
               createInfo.PlayerReceiver.StatModifierReceiver,
               createInfo.IPet
           ));
    }
}
