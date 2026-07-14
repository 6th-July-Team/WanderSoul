using UnityEngine;

public interface IDamageable
{
    void TakeDamage(DamageInfo damageInfo);
}

public struct DamageInfo
{
    // 기본 옵션
    public float DamageAmount;
    public Vector3 HitDirection;
    public DamageType DamageType;

    // 추가 옵션
    public float AdditionalDamage;

    public DamageInfo(float damageAmount, Vector3 hitDirection, DamageType damageType, float additionalDamage = 0)
    {
        DamageAmount = damageAmount;
        HitDirection = hitDirection;
        DamageType = damageType;
        AdditionalDamage = additionalDamage;
    }
}