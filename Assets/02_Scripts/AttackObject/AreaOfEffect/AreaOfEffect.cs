using UnityEngine;

public class AreaOfEffect : MonoBehaviour
{
    private Vector3 _centerPos;

    private float _damage;
    private float _radius;

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

        _damageType = aoeStruct.DamageType;
        _targetType = aoeStruct.TargetType;

        _visualPath = aoeStruct.VisualPath;


        CreateVisual();
        BurstDamage();
    }

    private void BurstDamage()
    {
        int targetCount = SearchUtil.FindTargetSphere(transform.position, _radius, LayerMask.GetMask("Enemy"), _targets);


        for (int i = 0; i < targetCount; i++)
        {
            IDamageable damageableTarget = _targets[i].GetComponent<IDamageable>();

            var direction = (_centerPos - damageableTarget.Position).normalized;

            DamageInfo damageInfo = new(_damage, direction, _damageType);

            if (SearchUtil.IsValidTarget(damageableTarget) == false)
            {
                continue;
            }

            damageableTarget.TakeDamage(damageInfo);
        }
    }

    private void CreateVisual()
    {
        GameObject visualPrefab = Utils.ResourcesLoad<GameObject>(_visualPath);

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

public struct AreaOfEffectStruct
{
    public Vector3 CenterPos;

    public float Damage;
    public float Radius;

    public DamageType DamageType;
    public TargetType TargetType;

    public string VisualPath;

    public AreaOfEffectStruct(Vector3 centerPos
        , float damage, float radius
        , DamageType damageType, TargetType targetType
        , string visualPath)
    {
        CenterPos = centerPos;

        Damage = damage;
        Radius = radius;

        DamageType = damageType;
        TargetType = targetType;

        VisualPath = visualPath;
    }
}