using UnityEngine;

public class ScholarSpecialExecution : IPlayerSkillExecution, ISkillRangeCheckable
{
    private SkillRangeIndicator _skillRangeIndicator;
    private Collider[] _targets = new Collider[32];

    public void CheckSkillRange(PlayerSkillUseContext context)
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

    public void Execute(PlayerSkillUseContext context, float damage)
    {
        _skillRangeIndicator.Hide();

        PetElement element = GameManager.PetParty.GetPriorityPetElement();

        // TODO: element에 따라 이펙트만 다르게 보여주기.
        // 데이터 드리븐으로 스킬 범위 가져 오기
        float attackRange = 7f;

        SearchUtil.FindNearestTarget(context.AimWorldPoint, attackRange, LayerMask.GetMask("Enemy"), _targets);

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

        GameObject prefab =
            Utils.ResourcesLoad<GameObject>("Decal");

        GameObject decal = Object.Instantiate(prefab);

        _skillRangeIndicator =
            decal.GetComponent<SkillRangeIndicator>();

        _skillRangeIndicator.Hide();
    }
}
