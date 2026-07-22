using UnityEngine;

public class PetMeleeSkill : IPetActiveSkillExecution
{
    private Collider[] _targets = new Collider[32];

    public void Execute(PetSkillUseContext context)
    {
        var target = SearchUtil.FindNearestSphere(context.PetPos, context.PetActiveSkillData.CastRange
            , LayerMask.GetMask("Enemy"), _targets);

        Vector3 direct = (target.Position - context.PetPos).normalized;

        var halfExtents = new Vector3(context.PetActiveSkillData.Radius, context.PetActiveSkillData.Radius, context.PetActiveSkillData.Radius);

        int count = SearchUtil.FindTargetBox(context.PetPos, halfExtents
            , LayerMask.GetMask("Enemy"), _targets, direct);

        DamageInfo damageInfo = new DamageInfo(context.PetActiveSkillData.Power, direct
            , context.PetActiveSkillData.GetDamageType());

        for (int i = 0; i < count; i++)
        {
            var targetable = _targets[i].GetComponent<IDamageable>();
            if (targetable != null)
            {
                targetable.TakeDamage(damageInfo);
            }
        }
    }
}
