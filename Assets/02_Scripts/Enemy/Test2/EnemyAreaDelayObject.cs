using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAreaDelayObject : MonoBehaviour
{
    [SerializeField] private GameObject TelegraphObject;
    [SerializeField] private GameObject EruptionObject;

    // [촉수 공격 수정] 데미지 판정과 분리된 촉수 시각 연출 컴포넌트입니다.
    // 연결하지 않으면 기존 AreaDelayed 공격처럼 동작하도록 선택 항목으로 두었습니다.
    [Header("촉수 공격 연출")]
    [SerializeField] private TentacleEruptionEffect TentacleEffect;

    public event Action<IDamageable, Vector3> OnDamageableTargetHited;
    private float _gizmoRadius;

    private void Awake()
    {
        bool hasObject = true;

        if (TelegraphObject == null)
        {
            Logger.LogError($"{this.name}: {nameof(TelegraphObject)}가 없습니다!!");
            hasObject = false;
        }

        if (EruptionObject == null)
        {
            Logger.LogError($"{this.name}: {nameof(EruptionObject)}가 없습니다!!");
            hasObject = false;
        }

        if (hasObject == false)
        {
            return;
        }

        TelegraphObject.SetActive(true);
        EruptionObject.SetActive(false);

        // [촉수 공격 수정] 공격 프리팹이 생성될 때 촉수 모델을 지면 아래에 준비합니다.
        if (TentacleEffect != null)
        {
            TentacleEffect.Prepare();
        }
    }

    public void Deploy(float radius, float delayTime)
    {
        _gizmoRadius = radius;
        float diameter = radius * 2;

        Vector3 telegraphScale = TelegraphObject.transform.localScale;
        TelegraphObject.transform.localScale = new Vector3(telegraphScale.x * diameter, telegraphScale.y + 0.01f, telegraphScale.z * diameter);

        AttackAfterDelay(radius, delayTime).Forget();
    }

    private async UniTaskVoid AttackAfterDelay(float radius, float delayTime)
    {
        // [촉수 공격 수정] AreaDelayTime의 마지막 RiseDuration 동안 촉수가 먼저 올라옵니다.
        // 따라서 기존 데이터의 "N초 후 데미지" 시점은 변경되지 않습니다.
        if (TentacleEffect != null)
        {
            float riseDuration = Mathf.Min(TentacleEffect.RiseDuration, delayTime);
            float waitBeforeRise = Mathf.Max(0f, delayTime - riseDuration);

            await Delay(waitBeforeRise);

            EruptionObject.SetActive(true);
            await TentacleEffect.PlayRise(riseDuration);
        }
        else
        {
            // [촉수 공격 수정] 촉수 컴포넌트가 없는 기존 프리팹도 계속 동작하는 안전장치입니다.
            await Delay(delayTime);
            EruptionObject.SetActive(true);
        }

        TelegraphObject.SetActive(false);

        // [촉수 공격 수정] 촉수가 완전히 올라온 정확한 시점에 충격 파티클과 데미지를 실행합니다.
        if (TentacleEffect != null)
        {
            TentacleEffect.PlayImpactAndHide().Forget();
        }

        ApplyAreaDamage(radius);

        // [촉수 공격 수정] 하강 연출이 끝나기 전에 프리팹이 제거되지 않도록 시간을 보정합니다.
        float destroyDelay = 1.5f;

        if (TentacleEffect != null)
        {
            destroyDelay = Mathf.Max(destroyDelay, TentacleEffect.DurationAfterImpact);
        }

        Destroy(gameObject, destroyDelay);
    }

    // [촉수 공격 수정] 기존 데미지 코드를 별도 메서드로 분리해 연출 타이밍을 읽기 쉽게 했습니다.
    private void ApplyAreaDamage(float radius)
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, radius);

        HashSet<IDamageable> alreadyAttackedTarget = new();

        foreach (Collider hit in hits)
        {
            IDamageable damageableTarget = hit.GetComponent<IDamageable>();

            if (damageableTarget == null || damageableTarget.IsAlive == false)
            {
                continue;
            }

            if (damageableTarget.EntityType == EntityType.Enemy)
            {
                continue;
            }

            if (alreadyAttackedTarget.Contains(damageableTarget))
            {
                continue;
            }

            Vector3 hitDirection = (damageableTarget.Position - transform.position).normalized;

            OnDamageableTargetHited?.Invoke(damageableTarget, hitDirection);
            alreadyAttackedTarget.Add(damageableTarget);
        }
    }

    // [촉수 공격 수정] 생성된 공격 오브젝트가 제거되면 대기 작업도 함께 취소됩니다.
    private async UniTask Delay(float seconds)
    {
        if (seconds <= 0f)
        {
            return;
        }

        await UniTask.Delay(
            TimeSpan.FromSeconds(seconds),
            cancellationToken: this.GetCancellationTokenOnDestroy());
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _gizmoRadius);
    }
}
