using UnityEngine;

public class ScholarAirArrow : IElementArrowVariant
{
    public void Fire(PlayerSkillUseContext context, float damage, PlayerSkillData SkillData)
    {
        // 빠른 번개 화살을 발사해서 전기 피해를 입힌다.
        // 단발의 피해량은 낮지만 공격 주기와 투사체 속도가 빠르다.

        Projectile projectileInstance = Object.Instantiate(
                Resources.Load<Projectile>("Player/Skill/Electric Arrow")
                , context.Owner.position + context.AimDirection * 1f, Quaternion.identity);

        projectileInstance.Init(new ProjectileStruct
        (
            SkillData.ProjectileSpeed, SkillData.Power, SkillData.Duration, context.AimDirection
            , SkillData.GetDamageType(), SkillData.GetTargetType()
        ));
    }
}
