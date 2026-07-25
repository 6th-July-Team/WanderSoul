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

        // 펫 임시 주석 TODO(김익환): 펫 스킬 보완 시 처리
        //Projectile projectileInstance = GameObject.Instantiate(
        //        Resources.Load<Projectile>("Pet/TestProjectile")
        //        , context.PetPos, Quaternion.identity);

        //projectileInstance.Init(new ProjectileStruct
        //(
        //    context.PetActiveSkillData.ProjectileSpeed, context.PetActiveSkillData.Power, context.PetActiveSkillData.Duration
        //    , direct, context.PetActiveSkillData.GetDamageType(), context.PetActiveSkillData.GetTargetType()
        //    , context.PetActiveSkillData.VFXPath
        //));
    }
}
