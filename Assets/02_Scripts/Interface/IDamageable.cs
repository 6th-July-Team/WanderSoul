using UnityEngine;

public interface IDamageable
{
    void TakeDamage(DamageInfo damageInfo);
}

//public struct DamageInfo
//{
//    // 넉백값, 상태이상 정보 등등 추가 가능
//    public float DamageAmount;
//    public Vector3 DamageDirection;
//    public DamageInfo(float damageAmount, Vector3 damageDirection)
//    {
//        DamageAmount = damageAmount;
//        DamageDirection = damageDirection;
//    }
//}