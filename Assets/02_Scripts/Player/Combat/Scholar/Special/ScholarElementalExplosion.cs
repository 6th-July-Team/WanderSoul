using UnityEngine;

public class ScholarElementalExplosion : IPlayerSkillExecution, ISkillRangeCheckable
{
    private SkillRangeIndicator _skillRangeIndicator;
    private Collider[] _targets = new Collider[32];

    public void CheckSkillRange(PlayerSkillUseContext context, PlayerSkillData SkillData)
    {
        InitIndicator();

        _skillRangeIndicator.Show(context.AimWorldPoint, 7f);
    }

    public void HideSkillRange()
    {
        if (_skillRangeIndicator == null)
            return;

        _skillRangeIndicator.Hide();
    }

    public void Execute(PlayerSkillUseContext context, PlayerSkillData SkillData, float damage)
    {
        _skillRangeIndicator.Hide();

        PetElement element = GameManager.PetParty.GetPriorityPetElement();

        float attackRange = SkillData.Radius;

        Vector3 skillCenter = context.AimWorldPoint;
        skillCenter.y += attackRange;
        SearchUtil.FindNearestSphere(skillCenter, attackRange, LayerMask.GetMask("Enemy"), _targets);

        DamageInfo damageInfo = new DamageInfo(damage, context.AimDirection
            , Utils.GetTypeByPetElement(element));

        foreach (var target in _targets)
        {
            if (target == null)
                continue;

            target.GetComponent<IDamageable>().TakeDamage(damageInfo);
        }
    }

    private void InitIndicator()
    {
        if (_skillRangeIndicator != null)
            return;

        GameObject decal = Object.Instantiate(Utils.ResourcesLoad<GameObject>("Decal"));
        _skillRangeIndicator = decal.GetComponent<SkillRangeIndicator>();

        _skillRangeIndicator.Hide();
    }
}
