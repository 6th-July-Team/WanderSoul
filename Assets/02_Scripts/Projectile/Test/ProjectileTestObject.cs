using UnityEngine;

public enum ProjectileType
{
    ShcolarFireProjectile,

}

public class ProjectileTestObject : MonoBehaviour
{
    [SerializeField] private GameObject Prefab_Projectile;
    [SerializeField] private Transform Transform_ShootPosition;

    [SerializeField] private int ProjectileSpeed;
    [SerializeField] private float ProjectileDamage;
    [SerializeField] private float ProjectileDuration;

    [SerializeField] private float ProjectileAdditionalDamage;
    [SerializeField] private float ProjectileBoomRange;
    [SerializeField] private int ProjectilePierce;

    [SerializeField] private DamageType ProjectileDamageType;
    [SerializeField] private TargetType TragetType;
    [SerializeField] private ProjectileType ProjectileType;

    [SerializeField] private float FireSecond;
    private float _time = 0;
    
    [SerializeField] private bool IsCreateProjectile;


    private void Awake()
    {
    }

    private void Update()
    {
        if (IsCreateProjectile)
        {
            _time += Time.deltaTime;

            if (_time >= FireSecond)
            {
                CreateProjectileAndFire();
                _time = 0;
            }
        }
    }

    private void CreateProjectileAndFire()
    {
        GameObject projectile = Instantiate(Prefab_Projectile, Transform_ShootPosition.position, Quaternion.identity);

        Projectile projectileComponent = projectile.GetComponent<Projectile>();

        if (projectileComponent == null)
        {
            Logger.LogError("생성한 Proejctile에 ShcolarFireProjectile가 없습니다.");
        }


        ProjectileStruct data = new ProjectileStruct(ProjectileSpeed, ProjectileDamage, ProjectileDuration, Vector3.forward, ProjectileDamageType, TragetType, ProjectileAdditionalDamage, continuousDamage: false, radius: ProjectileBoomRange, pierce: ProjectilePierce);
        projectileComponent.Init(data);
    }
}
