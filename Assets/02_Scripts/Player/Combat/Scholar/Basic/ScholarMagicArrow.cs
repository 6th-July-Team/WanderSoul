using UnityEngine;

public class ScholarMagicArrow : IElementArrowVariant
{
    private int _castCount = 1;
    private PlayerSkillData _upgradeSkillData;


    public void Fire(PlayerSkillUseContext context, float damage, PlayerSkillData SkillData)
    {
        if (_castCount % 4 == 0)
        {
            GameManager.Pool.SpawnFromPool<Projectile>("PlayerProjectile", context.BasicAttackAnchor.position)
                .Init(new ProjectileStruct
                (
                    SkillData.ProjectileSpeed, SkillData.Power, SkillData.Duration
                    , context.AimWorldPoint - context.BasicAttackAnchor.position
                    , SkillData.GetDamageType(), SkillData.GetTargetType()
                    , SkillData.VFXPath
                ));
        }
        else
        {
            if(_upgradeSkillData == null)
            {
                _upgradeSkillData = GameManager.DataTable.GetPlayerSkillData(SkillData.Id + "_upgrade");
            }

            GameManager.Pool.SpawnFromPool<Projectile>("PlayerProjectile", context.BasicAttackAnchor.position)
                .Init(new ProjectileStruct
                (
                    _upgradeSkillData.ProjectileSpeed, _upgradeSkillData.Power, _upgradeSkillData.Duration
                    , context.AimWorldPoint - context.BasicAttackAnchor.position
                    , _upgradeSkillData.GetDamageType(), _upgradeSkillData.GetTargetType()
                    , _upgradeSkillData.VFXPath
                ));
        }

        _castCount++;
    }
}
