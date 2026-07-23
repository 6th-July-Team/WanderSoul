using UnityEngine;

public class ScholarFireArrow : IElementArrowVariant
{
    public void Fire(PlayerSkillUseContext context, float damage, PlayerSkillData SkillData)
    {
        // 화염 화살을 발사해서 화염 피해를 입힌다.
        // 대상에 닿으면 폭발하여 넓은 범위의 적에게 피해를 준다.

        Projectile projectileInstance = Object.Instantiate(
                Resources.Load<Projectile>("Player/Skill/Fire Arrow")
                , context.Owner.position + context.AimDirection * 1f, Quaternion.identity);

        projectileInstance.Init(new ProjectileStruct
        (
            SkillData.ProjectileSpeed, SkillData.Power, SkillData.Duration, context.AimDirection
            , SkillData.GetDamageType(), SkillData.GetTargetType()
            , SkillData.ExtraDamage, radius: SkillData.Radius
        ));
    }
}
