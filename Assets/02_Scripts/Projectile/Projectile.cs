using UnityEngine;

public class Projectile : MonoBehaviour
{
    // 기본적인 옵션
    protected float _speed;
    protected float _damage;
    protected Vector3 _direction;

    protected DamageType _damageType = DamageType.None;
    protected EntityType _targetType;

    // 추가 옵션
    protected float _additionalDamage;

    private bool _continuousDamage;


    public void Init(ProjectileStruct projectileData)
    {
        // 기본 옵션
        _speed = projectileData.Speed;
        _damage = projectileData.Damage;
        _direction = projectileData.Direction;
        _damageType = projectileData.DamageType;
        _targetType = projectileData.TargetType;

        // 추가 옵션
        _additionalDamage = projectileData.AdditionalDamage;
        _continuousDamage = projectileData.ContinuousDamage;
    }

    public void Update()
    {
        transform.position += _direction * _speed * Time.deltaTime;

    }

    private void OnTriggerEnter(Collider other)
    {
        // TODO: 추후 다른 방식으로 변경,
        // 저번에 IDamageable : ITargetable로 변경한다 했으니, 거기 EntityType 보면 될 듯
        // 아래는 플레이어만을 위한 코드임.
        // Target Type을 받아서 이를 설정해야함.

        if (_continuousDamage)
            return;

        if(other.CompareTag("Enemy"))
        {
            if (other.TryGetComponent(out IDamageable damageable))
            {
                var damageinfo = new DamageInfo(_damage, _direction, _damageType);

                damageable.TakeDamage(damageinfo);
                HitEffect();
                Destroy(gameObject);
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(_continuousDamage)
        {
            if (other.CompareTag("Enemy"))
            {
                if (other.TryGetComponent(out IDamageable damageable))
                {
                    var damageinfo = new DamageInfo(_damage, _direction, _damageType);

                    damageable.TakeDamage(damageinfo);
                    HitEffect();
                    Destroy(gameObject);
                }
            }
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
    public EntityType TargetType;

    // 추가 옵션
    public float AdditionalDamage;
    public bool ContinuousDamage;

    public ProjectileStruct(float speed, float damage, Vector3 direction
        , DamageType damageType, EntityType targetType
        , float additionalDamage = 0, bool continuousDamage = false)
    {
        Speed = speed;
        Damage = damage;
        Direction = direction;
        DamageType = damageType;
        TargetType = targetType;

        AdditionalDamage = additionalDamage;
        ContinuousDamage = continuousDamage;
    }
}