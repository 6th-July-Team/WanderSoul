using System;
using UnityEngine;

public class PetProjectileSkill : IPetActiveSkillExecution
{
    private Collider[] _targets = new Collider[32];

    public void Execute(PetSkillUseContext context/*, Action OnEndSkill*/)
    {
        var target = SearchUtil.FindNearestSphere(context.PetPos, context.PetActiveSkillData.CastRange
            , LayerMask.GetMask("Enemy"), _targets);

        if(null == target)
        {
            Debug.Log("No Target");
            return;
        }

        Vector3 direct = (target.Position - context.PetPos).normalized;

        Projectile projectileInstance = GameObject.Instantiate(
                Resources.Load<Projectile>("TestProjectile")
                , context.PetPos, Quaternion.identity);

        projectileInstance.Init(new ProjectileStruct
        {
            Speed = 5f,
            Damage = context.PetActiveSkillData.Power,
            Direction = direct,
            DamageType = context.PetActiveSkillData.GetDamageType(),
            TargetType = context.PetActiveSkillData.GetTargetType()
        });
    }
}
