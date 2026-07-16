using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(BoxCollider))]
public class EnemyAttackHitbox : MonoBehaviour
{
    private BoxCollider _collider;

    private readonly List<IDamageable> _targetList = new();
    public IReadOnlyList<IDamageable> TargetList => _targetList;

    private void Awake()
    {
        _collider = GetComponent<BoxCollider>();
        _collider.isTrigger = true;
    }

    public void SetRange(float range)
    {
        Vector3 size = _collider.size;
        size.z = range;
        _collider.size = size;

        Vector3 center = _collider.center;
        center.z = range * 0.5f;
        _collider.center = center;
    }

    private void OnTriggerEnter(Collider other)
    {
        IDamageable damageableTarget = other.GetComponentInParent<IDamageable>();

        if (damageableTarget == null || damageableTarget.EntityType == EntityType.Enemy || _targetList.Contains(damageableTarget))
        {
            return;
        }

        _targetList.Add(damageableTarget);
    }

    private void OnTriggerExit(Collider other)
    {
        IDamageable damageableTarget = other.GetComponentInParent<IDamageable>();

        if (damageableTarget == null)
        {
            return;
        }

        _targetList.Remove(damageableTarget);
    }
}
