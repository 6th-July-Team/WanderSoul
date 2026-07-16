using UnityEngine;

public class MercenarySlash : IPlayerSkillExecution
{
    Collider[] _targets = new Collider[32];
    public void Execute(PlayerSkillUseContext context, PlayerSkillData SkillData, float damage)
    {
        // 곡검을 휘둘러 일정 범위를 공격한다.
        // 검에 맞은 적은 일정 확률로 출혈 상태 이상에 걸린다.
        float radius = 1.5f;
        Vector3 posOffset = context.AimDirection * radius;
        posOffset.y = 1f; // 플레이어 모델 위치 고려.

        Vector3 halfExtents = new Vector3(radius, 1.5f, radius);

        int count = SearchUtil.FindTargetBox(context.Owner.position + posOffset
            , halfExtents/*SkillData.Radius*/
            , LayerMask.GetMask("Enemy")
            , _targets
            ,context.AimDirection);


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
