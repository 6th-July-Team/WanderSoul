using UnityEngine;

public class ScholarWaterArrow : IElementArrowVariant
{
    public void Fire(PlayerSkillUseContext context, float damage, PlayerSkillData SkillData)
    {
        // 냉기 화살을 발사해서 냉기 피해를 입힌다.
        // 일정 수의 적을 관통하며, 피격된 적의 이동 속도를 잠시 감소시킨다.

        Projectile projectileInstance = Object.Instantiate(
                Resources.Load<Projectile>("TestProjectileFire")
                , context.Owner.position + context.AimDirection * 1f, Quaternion.identity);

        projectileInstance.Init(new ProjectileStruct
        {
            Speed = 5f,
            Damage = damage,
            Direction = context.AimDirection,
            AdditionalDamage = damage * 2,
            DamageType = SkillData.GetDamageType(),
            TargetType = SkillData.GetTargetType()

        });
    }
}
