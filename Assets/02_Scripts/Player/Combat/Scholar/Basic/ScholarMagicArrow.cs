using UnityEngine;

public class ScholarMagicArrow : IElementArrowVariant
{
    private int _castCount = 1;
    private PlayerSkillData _upgradeSkillData;

    private float _basicDamage = 0f;
    private float _upgradeDamage = 0f;

    public void Fire(PlayerSkillUseContext context, float damage, PlayerSkillData SkillData)
    {
        if (_upgradeSkillData == null)
        {
            _upgradeSkillData = GameManager.DataTable.GetPlayerSkillData(SkillData.Id + "_upgrade");
        }


        if (_basicDamage == 0f)
            _basicDamage = context.PlayerSkillModifier.GetValue(SkillData.Id, SkillSlot.Basic, SkillValueType.Power, SkillData.Power);

        if (_upgradeDamage == 0f)
            _upgradeDamage = context.PlayerSkillModifier.GetValue(_upgradeSkillData.Id, SkillSlot.Basic, SkillValueType.Power, _upgradeSkillData.Power);



        if (_castCount % 4 != 0)
        {
            GameManager.Pool.SpawnFromPool<Projectile>("ProjectileHusks", context.BasicAttackAnchor.position)
                .Init(new ProjectileStruct
                (
                    SkillData.ProjectileSpeed, _basicDamage, SkillData.Duration
                    , context.AimWorldPoint - context.BasicAttackAnchor.position
                    , SkillData.GetDamageType(), SkillData.GetTargetType()
                    , SkillData.VFXPath
                ));
        }
        else
        {

            GameManager.Pool.SpawnFromPool<Projectile>("ProjectileHusks", context.BasicAttackAnchor.position)
                .Init(new ProjectileStruct
                (
                    _upgradeSkillData.ProjectileSpeed, _upgradeDamage, _upgradeSkillData.Duration
                    , context.AimWorldPoint - context.BasicAttackAnchor.position
                    , _upgradeSkillData.GetDamageType(), _upgradeSkillData.GetTargetType()
                    , _upgradeSkillData.VFXPath
                ));
        }

        _castCount++;
    }
}
