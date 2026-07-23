using UnityEngine;

public class Projectile : MonoBehaviour
{
    // 기본적인 옵션
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

    [SerializeField] private Transform _visualRoot;
    private string _visualPath;
    private string _hitEffectPath;

    public void Init(ProjectileStruct projectileData)
    {
        // 기본 옵션
        _speed = projectileData.Speed;
        _damage = projectileData.Damage;
        _duration = projectileData.Duration;

        _direction = projectileData.Direction;
        _damageType = projectileData.DamageType;
        _targetType = projectileData.TargetType;

        _visualPath = projectileData.VisualPath;
        _hitEffectPath = projectileData.HitEffectPath;

        // 추가 옵션
        _extraDamage = projectileData.AdditionalDamage;
        _continuousDamage = projectileData.ContinuousDamage;
        _radius = projectileData.Radius;
        _pierce = projectileData.Pierce;

        if (_duration > 0)
        {
            Destroy(this.gameObject, _duration);
        }

        CreateVisual();
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

        if (other.CompareTag("Enemy"))
        {

            if (other.TryGetComponent(out IDamageable damageable))
            {

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
                }
            }
        }
    }

    private void HitEffect(Vector3 position)
    {
        if(_hitEffectPath == null)
        {
            return;
        }

        GameObject hitPrefab = Resources.Load<GameObject>(_hitEffectPath);
        if(hitPrefab == null)
        {
            Debug.LogError($"Projectile 타격 이펙트를 불러오지 못했습니다. Path: {_hitEffectPath}");
            return;
        }

        GameObject hitObject = Instantiate(hitPrefab, position, Quaternion.identity);

        if (!hitObject.TryGetComponent(out ParticleComponent hitVisual))
        {
            Debug.LogError($"{hitPrefab.name} 루트에 ParticleComponent 없습니다.");

            Destroy(hitObject);
            return;
        }

        float scale = Mathf.Max(_radius, 1f);

        hitVisual.SetScale(Vector3.one * scale);
        hitVisual.Play();

        Destroy(hitObject, hitVisual.ReleaseDelay);
    }

    private void BurstDamage()
    {
        int targetCount = SearchUtil.FindTargetSphere(transform.position, _radius, LayerMask.GetMask("Enemy"), _burstTarget);

        DamageInfo damageInfo = new(_extraDamage, _direction, _damageType);

        for (int i = 0; i < targetCount; i++)
        {
            IDamageable damageableTarget = _burstTarget[i].GetComponentInParent<IDamageable>();

            if (SearchUtil.IsValidTarget(damageableTarget) == false)
            {
                continue;
            }

            damageableTarget.TakeDamage(damageInfo);
        }
    }

    private void CreateVisual()
    {
        GameObject visualPrefab = Resources.Load<GameObject>(_visualPath);

        if (visualPrefab == null)
        {
            Debug.LogError($"Projectile 비주얼을 불러오지 못했습니다. Path: {_visualPath}");

            return;
        }

        var visualObject = Instantiate(visualPrefab, _visualRoot, false);

        visualObject.transform.localPosition = Vector3.zero;
        visualObject.transform.localRotation = Quaternion.identity;

        if (visualObject.TryGetComponent(out ParticleComponent visualComponent))
        {
            visualComponent.Play();
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

    public string VisualPath;

    // 추가 옵션
    public float AdditionalDamage; // 폭발 같은 추가적인 데미지
    public bool ContinuousDamage; // 틱마다 피해를 입히는가?
    public float Radius; // 폭발 범위 -> 없을 경우 기본값 0
    public int Pierce; // 관통력 -> 없을 경우 기본값 0
    public string HitEffectPath;

    public ProjectileStruct(float speed, float damage, float duration, Vector3 direction
        , DamageType damageType, TargetType targetType
        , string visualPath, string hitEffectPath = null
        , float extraDamage = 0, bool continuousDamage = false, float radius = 0, int pierce = 0)
    {
        Speed = speed;
        Damage = damage;
        Duration = duration;

        Direction = direction;
        DamageType = damageType;
        TargetType = targetType;

        VisualPath = visualPath;
        HitEffectPath = hitEffectPath;

        AdditionalDamage = extraDamage;
        ContinuousDamage = continuousDamage;

        Radius = radius;
        Pierce = pierce;
    }
}