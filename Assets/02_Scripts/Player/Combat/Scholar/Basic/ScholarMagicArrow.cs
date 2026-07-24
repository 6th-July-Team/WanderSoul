using UnityEngine;

public class ScholarMagicArrow : IElementArrowVariant
{
    private int _castCount = 1;

    public void Fire(PlayerSkillUseContext context, float damage, PlayerSkillData SkillData)
    {
        if (_castCount % 4 == 0)
        {
            // 강화 화살 발사 로직 추가
            Projectile projectileInstance = Object.Instantiate(Resources.Load<Projectile>("PlayerProjectile")
                , context.BasicAttackAnchor.position + context.AimDirection * 1f, Quaternion.identity);

            projectileInstance.Init(new ProjectileStruct
            (
                SkillData.ProjectileSpeed, SkillData.Power, SkillData.Duration, context.AimDirection
                , SkillData.GetDamageType(), SkillData.GetTargetType()
                , SkillData.VFXPath
            ));
        }
        else
        {
            Projectile projectileInstance = Object.Instantiate(Resources.Load<Projectile>("PlayerProjectile")
                , context.BasicAttackAnchor.position + context.AimDirection * 1f, Quaternion.identity);

            projectileInstance.Init(new ProjectileStruct
            (
                SkillData.ProjectileSpeed, SkillData.Power, SkillData.Duration, context.AimDirection
                , SkillData.GetDamageType(), SkillData.GetTargetType()
                , SkillData.VFXPath
            ));
        }

        _castCount++;
    }
}
