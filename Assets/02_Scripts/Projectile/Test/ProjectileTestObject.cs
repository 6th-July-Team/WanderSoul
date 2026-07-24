using UnityEngine;

public class ProjectileTestObject : MonoBehaviour
{
    [SerializeField] private GameObject Prefab_Projectile;
    [SerializeField] private GameObject Target;
    [SerializeField] private Transform Transform_ShootPosition;

    [SerializeField] private float ProjectileSpeed;
    [SerializeField] private float ProjectileLifeTime;

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

        EnemyProjectileObject script = projectile.GetComponent<EnemyProjectileObject>();

        if(script == null)
        {
            Logger.LogError("오류 발생! EnemyProjectileObject 스크립트가 없습니다!!");
            return;
        }

        script.Launch(Target, ProjectileSpeed, ProjectileLifeTime);
    }
}
