using UnityEngine;

public static class SearchUtil
{
    public static ITargetable FindNearestTarget(Vector3 center, float radius, LayerMask targetLayerMask, Collider[] buffer)
    {
        int count = Physics.OverlapSphereNonAlloc(center, radius, buffer, targetLayerMask);

        ITargetable nearestTarget = null;
        float nearestSqrDistance = radius * radius;

        for (int i = 0; i < count; i++)
        {
            Collider col = buffer[i];
            if (col == null)
                continue;

            ITargetable target = col.GetComponentInParent<ITargetable>();

            if (!IsValidTarget(target))
                continue;

            float sqrDistance = (target.Position - center).sqrMagnitude;

            if (sqrDistance < nearestSqrDistance)
            {
                nearestSqrDistance = sqrDistance;
                nearestTarget = target;
            }

            buffer[i] = null;
        }

        return nearestTarget;
    }

    // Utils 내에서 사용
    public static bool IsValidTarget(ITargetable target)
    {
        return target != null && target.IsAlive;
    }
}