using UnityEngine;

public class MercenaryWhip : IPlayerSkillExecution
{
    Collider[] _targets = new Collider[64];
    public void Execute(PlayerSkillUseContext context, PlayerSkillData SkillData, float damage)
    {
        float radius = 2.5f;
        Vector3 posOffset = context.AimDirection * radius;
        posOffset.y = 1f; // 플레이어 모델 위치 고려.

        Vector3 halfExtents = new Vector3(radius, 1f, radius);

        int count = SearchUtil.FindTargetBox(context.Owner.position + posOffset
            , halfExtents/*SkillData.Radius*/
            , LayerMask.GetMask("Enemy")
            , _targets
            , context.AimDirection);

        DamageInfo damageInfo = new DamageInfo(1, context.AimDirection, DamageType.None);

        for (int i = 0; i < count; i++)
        {
            var target = _targets[i].GetComponent<IDamageable>();
            if (target != null)
            {
                target.TakeDamage(damageInfo);
            }
        }
    }
}
