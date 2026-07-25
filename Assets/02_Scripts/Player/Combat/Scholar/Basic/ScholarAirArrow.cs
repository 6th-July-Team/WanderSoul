using UnityEngine;

public class ScholarAirArrow : IElementArrowVariant
{
    public void Fire(PlayerSkillUseContext context, float damage, PlayerSkillData SkillData)
    {
        GameManager.Pool.SpawnFromPool<Projectile>("PlayerProjectile", context.BasicAttackAnchor.position)
            .Init(new ProjectileStruct
        (
            SkillData.ProjectileSpeed, SkillData.Power, SkillData.Duration
            , context.AimWorldPoint - context.BasicAttackAnchor.position
            , SkillData.GetDamageType(), SkillData.GetTargetType()
            , SkillData.VFXPath
        ));
    }
}
