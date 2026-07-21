using UnityEngine;

public static class SearchUtil
{
    public static ITargetable FindNearestSphere(Vector3 center, float radius, LayerMask targetLayerMask, Collider[] buffer)
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

    public static int FindTargetSphere(Vector3 center, float radius, LayerMask targetLayerMask, Collider[] buffer)
    {
        int count = Physics.OverlapSphereNonAlloc(center, radius, buffer, targetLayerMask);
        DrawWireSphere(center, radius, 0.3f);
        return count;
    }

    public static int FindTargetBox(Vector3 center, Vector3 halfExtents, LayerMask targetLayerMask, Collider[] buffer, Vector3 attackDirection)
    {
        Quaternion rotation = Quaternion.LookRotation(attackDirection, Vector3.up);

        int count = Physics.OverlapBoxNonAlloc(center, halfExtents, buffer, rotation, targetLayerMask);

        DrawWireBox(center, halfExtents, rotation, 0.3f);

        return count;
    }


    // Utils 내에서 사용
    public static bool IsValidTarget(ITargetable target)
    {
        return target != null && target.IsAlive;
    }



    #region Debug

    public static void DrawWireSphere(Vector3 center, float radius, float duration)
    {
        int segments = 24;
        float angle = 360f / segments;
        for (int i = 0; i < segments; i++)
        {
            float currentAngle = angle * i;
            float nextAngle = angle * (i + 1);
            Vector3 currentPoint = center + new Vector3(Mathf.Cos(currentAngle * Mathf.Deg2Rad), 0, Mathf.Sin(currentAngle * Mathf.Deg2Rad)) * radius;
            Vector3 nextPoint = center + new Vector3(Mathf.Cos(nextAngle * Mathf.Deg2Rad), 0, Mathf.Sin(nextAngle * Mathf.Deg2Rad)) * radius;
            Debug.DrawLine(currentPoint, nextPoint, Color.green, duration);
        }
    }

    public static void DrawWireBox(Vector3 center, Vector3 halfExtents, Quaternion rotation, float duration)
    {
        Vector3 p0 = center + rotation * new Vector3(
            -halfExtents.x,
            -halfExtents.y,
            -halfExtents.z);

        Vector3 p1 = center + rotation * new Vector3(
            halfExtents.x,
            -halfExtents.y,
            -halfExtents.z);

        Vector3 p2 = center + rotation * new Vector3(
            halfExtents.x,
            -halfExtents.y,
            halfExtents.z);

        Vector3 p3 = center + rotation * new Vector3(
            -halfExtents.x,
            -halfExtents.y,
            halfExtents.z);

        Vector3 p4 = center + rotation * new Vector3(
            -halfExtents.x,
            halfExtents.y,
            -halfExtents.z);

        Vector3 p5 = center + rotation * new Vector3(
            halfExtents.x,
            halfExtents.y,
            -halfExtents.z);

        Vector3 p6 = center + rotation * new Vector3(
            halfExtents.x,
            halfExtents.y,
            halfExtents.z);

        Vector3 p7 = center + rotation * new Vector3(
            -halfExtents.x,
            halfExtents.y,
            halfExtents.z);

        Debug.DrawLine(p0, p1, Color.red, duration);
        Debug.DrawLine(p1, p2, Color.red, duration);
        Debug.DrawLine(p2, p3, Color.red, duration);
        Debug.DrawLine(p3, p0, Color.red, duration);

        Debug.DrawLine(p4, p5, Color.red, duration);
        Debug.DrawLine(p5, p6, Color.red, duration);
        Debug.DrawLine(p6, p7, Color.red, duration);
        Debug.DrawLine(p7, p4, Color.red, duration);

        Debug.DrawLine(p0, p4, Color.red, duration);
        Debug.DrawLine(p1, p5, Color.red, duration);
        Debug.DrawLine(p2, p6, Color.red, duration);
        Debug.DrawLine(p3, p7, Color.red, duration);
    }

    #endregion
}