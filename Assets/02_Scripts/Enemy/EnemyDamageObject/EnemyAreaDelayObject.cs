using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAreaDelayObject : MonoBehaviour
{
    [SerializeField] private GameObject TelegraphObject;
    [SerializeField] private GameObject EruptionObject;

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

        if(hasObject == false)
        {
            return;
        }

        Debug.LogWarning("장판 생성!!");

        TelegraphObject.SetActive(true);
        EruptionObject.SetActive(false);
    }

    public void Deploy(float radius, float delayTime)
    {
        _gizmoRadius = radius;
        float diameter = radius * 2;

        Vector3 telegraphScale = TelegraphObject.transform.localScale;
        TelegraphObject.transform.localScale = new Vector3(telegraphScale.x * diameter, telegraphScale.y + 0.01f, telegraphScale.z * diameter);

        //Vector3 EruptionScale = EruptionObject.transform.localScale;
        //EruptionObject.transform.localScale = new Vector3(diameter, diameter, diameter);

        AttackAfterDelay(radius, delayTime).Forget();
    }

    private async UniTaskVoid AttackAfterDelay(float radius, float delayTime)
    {
        await UniTask.Delay(TimeSpan.FromSeconds(delayTime));

        TelegraphObject.SetActive(false);
        EruptionObject.SetActive(true);

        Collider[] hits = Physics.OverlapSphere(transform.position, radius);

        HashSet<IDamageable> alreadyAttackedTarget = new();

        foreach (Collider hit in hits)
        {
            IDamageable damageableTarget = hit.GetComponentInParent<IDamageable>();

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


            Vector3 hitDirection = (damageableTarget.Position - this.transform.position).normalized;

            OnDamageableTargetHited?.Invoke(damageableTarget, hitDirection);
            alreadyAttackedTarget.Add(damageableTarget);
        }

        Destroy(gameObject, 1.5f);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _gizmoRadius);
    }
}
