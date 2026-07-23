using Unity.AppUI.UI;
using UnityEngine;

public class ScholarWaterArrow : IElementArrowVariant
{
    public void Fire(PlayerSkillUseContext context, float damage, PlayerSkillData SkillData)
    {
        // 냉기 화살을 발사해서 냉기 피해를 입힌다.
        // 일정 수의 적을 관통하며, 피격된 적의 이동 속도를 잠시 감소시킨다.

        Projectile projectileInstance = Object.Instantiate(
                Resources.Load<Projectile>("Player/Skill/Ice Arrow")
                , context.Owner.position + context.AimDirection * 1f, Quaternion.identity);

        projectileInstance.Init(new ProjectileStruct
        (
            SkillData.ProjectileSpeed, SkillData.Power, SkillData.Duration, context.AimDirection
            , SkillData.GetDamageType(), SkillData.GetTargetType()
        ));
    }
}
