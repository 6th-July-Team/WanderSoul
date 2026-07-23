using UnityEngine;

public class ScholarEarthArrow : IElementArrowVariant
{
    public void Fire(PlayerSkillUseContext context, float damage, PlayerSkillData SkillData)
    {
        // 무거운 암석 화살을 발사해서 물리 피해를 입힌다.
        // 투사체 속도와 공격 주기는 느리지만 높은 피해를 준다.

        Projectile projectileInstance = Object.Instantiate(
                Resources.Load<Projectile>("Player/Skill/Stone Arrow")
                , context.Owner.position + context.AimDirection * 1f, Quaternion.identity);

        projectileInstance.Init(new ProjectileStruct
        (
            SkillData.ProjectileSpeed, SkillData.Power, SkillData.Duration, context.AimDirection
            , SkillData.GetDamageType(), SkillData.GetTargetType()
        ));
    }
}
