using UnityEngine;

public class ScholarWaterArrow : IElementArrowVariant
{
    public void Fire(PlayerSkillUseContext context, float damage, PlayerSkillData SkillData)
    {
        Projectile projectileInstance = Object.Instantiate(Resources.Load<Projectile>("PlayerProjectile")
                , context.BasicAttackAnchor.position, Quaternion.identity);

        projectileInstance.Init(new ProjectileStruct
        (
            SkillData.ProjectileSpeed, SkillData.Power, SkillData.Duration
            , context.AimWorldPoint - context.BasicAttackAnchor.position
            , SkillData.GetDamageType(), SkillData.GetTargetType()
            , SkillData.VFXPath
        ));
    }
}
