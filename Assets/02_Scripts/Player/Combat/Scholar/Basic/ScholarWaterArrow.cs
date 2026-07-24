using UnityEngine;

public class ScholarWaterArrow : IElementArrowVariant
{
    public void Fire(PlayerSkillUseContext context, float damage, PlayerSkillData SkillData)
    {
        Projectile projectileInstance = Object.Instantiate(Resources.Load<Projectile>("PlayerProjectile")
                , context.Owner.position + context.AimDirection * 1f, Quaternion.identity);

        projectileInstance.Init(new ProjectileStruct
        (
            SkillData.ProjectileSpeed, SkillData.Power, SkillData.Duration, context.AimDirection
            , SkillData.GetDamageType(), SkillData.GetTargetType()
            , SkillData.VFXPath
        ));
    }
}
