

public static class PetSkillRegistor
{
    public static void RegisterAllActiveSkills(PetActiveSkillExecutionRegistry registry)
    {
        registry.Register("Projectile"
            , (createInfo) => new PetProjectileSkill
            (
                // PetProjectileSkill에 필요한 정보
            ));

        registry.Register("Area"
            , (createInfo) => new PetAreaEffectSkill
            (
                // Area 공격 유형에 대한 필요한 정보
            ));

        registry.Register("WindBlessing"
            , (createInfo) => new PetWindBlessingSkill
            (
                createInfo.StatusEffectMaker,
                createInfo.EffectData,
                createInfo.PlayerReceiver,
                createInfo.PlayerHealable
            ));
    }

    public static void RegisterAllPassiveSkills(PetPassiveSkillExecutionRegistry registry)
    {
        registry.Register("PetProjectile"
            , (createInfo) => new PetBuffPassive
            (
                createInfo.EffectData,
                createInfo.PetPassiveSkillData,
                createInfo.PlayerReceiver.StatModifierReceiver,
                createInfo.PetModifierReceiver
            ));
    }
}
