using UnityEngine;

public class AreaOfEffect : MonoBehaviour
{
    private Vector3 _centerPos;

    private float _damage;
    private float _radius;
    private float _vfxRadius;

    private DamageType _damageType = DamageType.None;
    private TargetType _targetType;

    private string _visualPath;


    [SerializeField] private Transform _visualRoot;

    // 캐싱
    private Collider[] _targets = new Collider[64];


    public void Init(AreaOfEffectStruct aoeStruct)
    {
        _centerPos = aoeStruct.CenterPos;

        _damage = aoeStruct.Damage;
        _radius = aoeStruct.Radius;
        _vfxRadius = aoeStruct.VFXRadius;

        _damageType = aoeStruct.DamageType;
        _targetType = aoeStruct.TargetType;

        _visualPath = aoeStruct.VisualPath;

        VFXSpawner.SpawnVFX(_visualPath, _centerPos, Quaternion.identity, _vfxRadius);

        BurstDamage();

        //Destroy(gameObject);
    }

    private void BurstDamage()
    {
        int targetCount = SearchUtil.FindTargetSphere(transform.position, _radius, LayerMask.GetMask("Enemy"), _targets);

        for (int i = 0; i < targetCount; i++)
        {
            IDamageable damageableTarget = _targets[i].GetComponent<IDamageable>();

            if (SearchUtil.IsValidTarget(damageableTarget) == false)
            {
                continue;
            }

            var direction = (damageableTarget.Position - _centerPos).normalized;

            DamageInfo damageInfo = new(_damage, direction, _damageType);

            damageableTarget.TakeDamage(damageInfo);
        }
    }
}

public struct AreaOfEffectStruct
{
    public Vector3 CenterPos;

    public float Damage;
    public float Radius;
    public float VFXRadius;

    public DamageType DamageType;
    public TargetType TargetType;

    public string VisualPath;

    public AreaOfEffectStruct(Vector3 centerPos
        , float damage, float radius, float vfxRadius
        , DamageType damageType, TargetType targetType
        , string visualPath)
    {
        CenterPos = centerPos;

        Damage = damage;
        Radius = radius;
        VFXRadius = vfxRadius;

        DamageType = damageType;
        TargetType = targetType;

        VisualPath = visualPath;
    }
}