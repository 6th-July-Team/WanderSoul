using UnityEngine;

public class Projectile : MonoBehaviour
{
    // 기본적인 옵션
    protected float _speed;
    protected float _damage;
    protected Vector3 _direction;
    protected DamageType _damageType = DamageType.None;

    // 추가 옵션
    protected float _additionalDamage;

    public void Init(ProjectileStruct projectileData)
    {
        // 기본 옵션
        _speed = projectileData.Speed;
        _damage = projectileData.Damage;
        _direction = projectileData.Direction;
        _damageType = projectileData.DamageType;

        // 추가 옵션
        _additionalDamage = projectileData.AdditionalDamage;
    }

    public void Update()
    {
        transform.position += _direction * _speed * Time.deltaTime;

    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out IDamageable damageable))
        {
            var damageinfo = new DamageInfo(_damage, _direction, _damageType);

            damageable.TakeDamage(damageinfo);
            HitEffect();
            Destroy(gameObject);
        }
    }

    protected virtual void HitEffect()
    {

    }
}

public struct ProjectileStruct
{
    // 기본 옵션
    public float Speed;
    public float Damage;
    public Vector3 Direction;
    public DamageType DamageType;

    // 추가 옵션
    public float AdditionalDamage;

    public ProjectileStruct(float speed, float damage, Vector3 direction, DamageType damageType, float additionalDamage = 0)
    {
        Speed = speed;
        Damage = damage;
        Direction = direction;
        DamageType = damageType;
        AdditionalDamage = additionalDamage;
    }
}