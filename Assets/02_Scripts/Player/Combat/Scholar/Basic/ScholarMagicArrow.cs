using UnityEngine;

public class ScholarMagicArrow : IElementArrowVariant
{
    private int _castCount = 1;

    public void Fire(PlayerSkillUseContext context, float damage, PlayerSkillData SkillData)
    {
        // 마력 화살을 발사해서 무속성 피해를 입힌다.
        // 매 4회 공격마다 더 강한 피해를 주고 적을 관통하는 강화 화살을 발사한다

        if (_castCount % 4 == 0)
        {
            // 강화 화살 발사 로직 추가
            Projectile projectileInstance = Object.Instantiate(
                Resources.Load<Projectile>("Player/Skill/Magic Arrow")
                , context.Owner.position + context.AimDirection * 1f, Quaternion.identity);

            projectileInstance.Init(new ProjectileStruct
            (
                SkillData.ProjectileSpeed, SkillData.Power, SkillData.Duration, context.AimDirection
                , SkillData.GetDamageType(), SkillData.GetTargetType()
            ));
        }
        else
        {
            Projectile projectileInstance = Object.Instantiate(
                Resources.Load<Projectile>("Player/Skill/Upgraded Magic Arrow")
                , context.Owner.position + context.AimDirection * 1f, Quaternion.identity);

            projectileInstance.Init(new ProjectileStruct
            (
                SkillData.ProjectileSpeed, SkillData.Power, SkillData.Duration, context.AimDirection
                , SkillData.GetDamageType(), SkillData.GetTargetType()
            ));
        }

        _castCount++;
    }
}
