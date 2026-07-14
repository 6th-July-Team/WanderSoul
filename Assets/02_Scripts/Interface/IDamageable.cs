using UnityEngine;

public interface IDamageable
{
    void TakeDamage(DamageInfo damageInfo);
}

public struct DamageInfo
{
    public float DamageAmount;
    public Vector3 HitDirection;

    public DamageInfo(float damageAmount, Vector3 hitDirection)
    {
        DamageAmount = damageAmount;
        HitDirection = hitDirection;
    }
}