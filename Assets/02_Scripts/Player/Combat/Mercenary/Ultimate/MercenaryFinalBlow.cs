using UnityEngine;

public class MercenaryFinalBlow : IPlayerSkillExecution, ISkillRangeCheckable
{
    private SkillRangeIndicator _skillRangeIndicator;

    public void Execute(PlayerSkillUseContext context, PlayerSkillData SkillData, float damage)
    {
        Vector3 yOffset = Vector3.up * 5f;

        Projectile projectileInstance = Object.Instantiate(
                Resources.Load<Projectile>("MercenaryFinalBlow")
                , yOffset + (context.Owner.position + context.AimDirection * 1f), Quaternion.identity);

        projectileInstance.Init(new ProjectileStruct
        {
            Speed = 10f,
            Damage = damage,
            Direction = context.AimDirection,
            DamageType = SkillData.GetDamageType(),
            TargetType = SkillData.GetTargetType(),
        });
    }
    
    public void CheckSkillRange(PlayerSkillUseContext context, PlayerSkillData SkillData)
    {
        InitIndicator();

        _skillRangeIndicator.Show(context.Owner.position, context.AimDirection, 10f, 5f);
    }

    public void HideSkillRange()
    {
        if (_skillRangeIndicator == null)
            return;

        _skillRangeIndicator.Hide();
    }

    private void InitIndicator()
    {
        if (_skillRangeIndicator != null)
            return;

        GameObject decal = Object.Instantiate(Utils.ResourcesLoad<GameObject>("DecalCube"));
        _skillRangeIndicator = decal.GetComponent<SkillRangeIndicator>();

        _skillRangeIndicator.Hide();
    }
}
