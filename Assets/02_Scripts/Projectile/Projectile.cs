using UnityEngine;

public class Projectile : MonoBehaviour
{
    // 기본적인 옵션
    [SerializeField] private GameObject Prefab_VFXHit; // 히트했을 때 나오는 오브젝트

    private float _speed;
    private float _damage;
    private float _duration;

    private Vector3 _direction;

    private DamageType _damageType = DamageType.None;
    private TargetType _targetType;

    // 추가 옵션
    private float _extraDamage;
    private bool _continuousDamage;
    private float _radius;
    private int _pierce;

    // 폭발형 전용
    private Collider[] _burstTarget = new Collider[32];

    public void Init(ProjectileStruct projectileData)
    {
        // 기본 옵션
        _speed = projectileData.Speed;
        _damage = projectileData.Damage;
        _duration = projectileData.Duration;

        _direction = projectileData.Direction;
        _damageType = projectileData.DamageType;
        _targetType = projectileData.TargetType;

        // 추가 옵션
        _extraDamage = projectileData.AdditionalDamage;
        _continuousDamage = projectileData.ContinuousDamage;
        _radius = projectileData.Radius;
        _pierce = projectileData.Pierce;

        if (_duration > 0)
        {
            Destroy(this.gameObject, _duration);
        }
    }

    private void Update()
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
        {
            return;
        }

        Debug.Log($"트리거 들어옴");


        if (other.CompareTag("Enemy"))
        {
            Debug.Log($"태그 통과");

            if (other.TryGetComponent(out IDamageable damageable))
            {
                Debug.Log($"IDamageable 통과");

                var damageinfo = new DamageInfo(_damage, _direction, _damageType);

                damageable.TakeDamage(damageinfo);

                Debug.Log($"{_damage}");

                HitEffect(transform.position);

                if (_pierce <= 0)
                {
                    Destroy(gameObject);
                }
                else
                {
                    _pierce--;
                }

                if (_radius > 0)
                {
                    BurstDamage();
                }
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (_continuousDamage)
        {
            if (other.CompareTag("Enemy"))
            {
                if (other.TryGetComponent(out IDamageable damageable))
                {
                    var damageinfo = new DamageInfo(_damage, _direction, _damageType);

                    damageable.TakeDamage(damageinfo);
                    HitEffect(transform.position);
                    Destroy(gameObject);
                }
            }
        }
    }

    private void HitEffect(Vector3 position)
    {
        if (Prefab_VFXHit != null)
        {
            GameObject vfxHit = Instantiate(Prefab_VFXHit, position, Quaternion.identity);
            IProjectileScaler projectileScaler = vfxHit.GetComponentInParent<IProjectileScaler>();

            if (projectileScaler != null)
            {
                float scale = Mathf.Max(_radius, 1); // 폭발 범위가 0일 경우 투사체 타격 이펙트의 크기를 1로 만듦

                // TODO : _radius를 받아 파티클 또는 폭발 오브젝트 크기를 키워야 함
            }

            Destroy(vfxHit, 2f);
        }
    }

    private void BurstDamage()
    {

        int targetCount = SearchUtil.FindTargetSphere(transform.position, _radius, LayerMask.GetMask("Enemy"), _burstTarget);

        for(int i = 0;  i < targetCount; i++)
        {
            IDamageable damageableTarget = _burstTarget[i].GetComponentInParent<IDamageable>();

            DamageInfo damageInfo = new(_extraDamage, _direction, _damageType);

            if (SearchUtil.IsValidTarget(damageableTarget) == false)
            {
                continue;
            }

            damageableTarget.TakeDamage(damageInfo);
        }
    }
}

public struct ProjectileStruct
{
    // 기본 옵션
    public float Speed;
    public float Damage;
    public float Duration;

    public Vector3 Direction;
    public DamageType DamageType;
    public TargetType TargetType;

    // 추가 옵션
    public float AdditionalDamage; // 폭발 같은 추가적인 데미지
    public bool ContinuousDamage; // 틱마다 피해를 입히는가?
    public float Radius; // 폭발 범위 -> 없을 경우 기본값 0
    public int Pierce; // 관통력 -> 없을 경우 기본값 0

    public ProjectileStruct(float speed, float damage, float duration, Vector3 direction, DamageType damageType, TargetType targetType,
            float extraDamage = 0, bool continuousDamage = false, float radius = 0, int pierce = 0)
    {
        Speed = speed;
        Damage = damage;
        Duration = duration;

        Direction = direction;
        DamageType = damageType;
        TargetType = targetType;

        AdditionalDamage = extraDamage;
        ContinuousDamage = continuousDamage;

        Radius = radius;
        Pierce = pierce;
    }
}