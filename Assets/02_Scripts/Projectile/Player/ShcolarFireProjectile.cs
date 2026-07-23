using System.Collections;
using UnityEngine;

// 화염 화살을 발사해서 화염 피해를 입힌다.
// 대상에 닿으면 폭발하여 넓은 범위의 적에게 피해를 준다.

public class ShcolarFireProjectile : Projectile
{
    [SerializeField] private bool isShowGizmo = false;
    private Collider[] _targets = new Collider[32];

    protected override void HitEffect(Vector3 position)
    {
        base.HitEffect(position);

        StartCoroutine(ShowGizmoRoutine());

        SearchUtil.FindTargetSphere(transform.position, _radius, LayerMask.GetMask("Enemy"), _targets);

        DamageInfo damageInfo = new DamageInfo(_additionalDamage, _direction, _damageType);

        foreach (var target in _targets)
        {
            if (target == null) 
                continue;

            target.GetComponent<IDamageable>().TakeDamage(damageInfo);
        }
    }

    private void OnDrawGizmos()
    {
        // isShowGizmo가 true일 때만 기즈모를 그림
        if (isShowGizmo)
        {
            Gizmos.color = Color.red; // 기즈모 색상 지정
            // 구체 기즈모를 해당 오브젝트 위치에 그림
            Gizmos.DrawSphere(transform.position, _radius);
        }
    }

    private IEnumerator ShowGizmoRoutine()
    {
        isShowGizmo = true;

        // 지정된 시간(gizmoDuration)만큼 대기
        yield return new WaitForSeconds(1f);

        isShowGizmo = false;
    }
}
