using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;
using UnityEngine.VFX;

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

    [Header("Visual")]
    [SerializeField] private Transform _visualRoot;
    private string _visualAddress;
    private string _hitEffectAddress;

    // 콜라이더
    [Header("Collider")]
    private CapsuleCollider _collider;
    [SerializeField] private Vector3 _hitboxCenter;
    [SerializeField] private float _hitboxRadius;
    [SerializeField] private float _hitboxLength;

    // 이펙트
    private ParticleComponent _effect;

    private CancellationTokenSource _token;
    private bool _isReleased;

    public void Init(ProjectileStruct projectileData)
    {
        // 기본 옵션
        _speed = projectileData.Speed;
        _damage = projectileData.Damage;
        _duration = projectileData.Duration;

        _direction = projectileData.Direction.normalized;
        _damageType = projectileData.DamageType;
        _targetType = projectileData.TargetType;

        _visualAddress = projectileData.VisualPath;
        _hitEffectAddress = projectileData.HitEffectPath;

        // 추가 옵션
        _extraDamage = projectileData.AdditionalDamage;
        _continuousDamage = projectileData.ContinuousDamage;
        _radius = projectileData.Radius;
        _pierce = projectileData.Pierce;

        //_hitboxCenter = projectileData.HitboxCenter;
        //_hitboxRadius = projectileData.HitboxRadius;
        //_hitboxLength = projectileData.HitboxLength;


        if (_direction.sqrMagnitude > 0f)
        {
            transform.rotation = Quaternion.LookRotation(_direction, Vector3.up);
        }

        DisposeToken();

        _isReleased = false;

        if (_duration > 0)
        {
            _token = new CancellationTokenSource();
            DespawnDelay(_token.Token).Forget();
        }

        _effect = VFXSpawner.SpawnAttachedVFX(_visualAddress, _visualRoot, Vector3.zero, Quaternion.identity);
    }

    private void Update()
    {
        transform.position += _direction * _speed * GameManager.Time.GameDeltaTime;
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

                HitEffect(transform.position);

                if (_pierce <= 0)
                {
                    ReleaseToPool();
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
        float scale = Mathf.Max(_radius, 1f);
        VFXSpawner.SpawnVFX(_hitEffectAddress, position, Quaternion.identity, scale);
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

    private async UniTaskVoid DespawnDelay(CancellationToken token)
    {
        bool isCanceled = await UniTask.Delay((int)(_duration * 1000f), cancellationToken: token)
            .SuppressCancellationThrow();

        if (isCanceled)
        {
            return;
        }

        ReleaseToPool();
    }

    private void ReleaseToPool()
    {
        if (_isReleased)
        {
            return;
        }

        _isReleased = true;

        DisposeToken();

        if (_effect != null)
        {
            _effect.ReleaseToPool();
            _effect = null;
        }

        GameManager.Pool.DespawnToPool(gameObject);
    }

    private void DisposeToken()
    {
        if (_token != null)
        {
            _token.Cancel();
            _token.Dispose();
            _token = null;
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