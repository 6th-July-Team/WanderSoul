using UnityEngine;

public class ScholarFireArrow : IElementArrowVariant
{
    public void Fire(PlayerSkillUseContext context, float damage, PlayerSkillData SkillData)
    {
        Projectile projectileInstance = Object.Instantiate(Resources.Load<Projectile>("PlayerProjectile")
                , context.BasicAttackAnchor.position + context.AimDirection * 1f, Quaternion.identity);

        projectileInstance.Init(new ProjectileStruct
        (
            SkillData.ProjectileSpeed, SkillData.Power, SkillData.Duration, context.AimDirection
            , SkillData.GetDamageType(), SkillData.GetTargetType()
            , SkillData.VFXPath
            , extraDamage: SkillData.ExtraDamage, radius: SkillData.Radius
        ));
    }
}
