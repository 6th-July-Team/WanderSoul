using UnityEngine;

public class TestPlayer : MonoBehaviour, IDamageable
{
    public bool IsAlive => true;
    public EntityType EntityType => EntityType.Player;
    public Transform Transform => transform;
    public Vector3 Position => transform.position;

    [SerializeField] private float Hp = 1000f;

    public void TakeDamage(DamageInfo damageInfo)
    {
        float damage = damageInfo.DamageAmount;

        Hp -= damage;
        Debug.Log($"남은 체력 = {Hp}");
    }
}
