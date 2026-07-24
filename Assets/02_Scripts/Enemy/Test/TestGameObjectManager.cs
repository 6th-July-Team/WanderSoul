using UnityEngine;

public class TestGameObjectManager : MonoBehaviour
{
    [SerializeField] private bool SpawnEnemy;
    [SerializeField] private float SpawnTime;
    private float _time = 0;

    [SerializeField] private Transform SpawnPointOne;
    [SerializeField] private Transform SpawnPointTwo;
    [SerializeField] private Transform SpawnPointThree;
    [SerializeField] private Transform SpawnPointFour;

    [SerializeField] private GameObject Player;
    [SerializeField] private GameObject Wagon;
    [SerializeField] private GameObject Pet;

    [SerializeField] private int MeleeEnemySummonCount;
    [SerializeField] private int ProjectileEnemySummonCount;
    [SerializeField] private int AreaDelayedEnemySummonCount;
    [SerializeField] private int MeleeWagonOnlyEnemySummonCount;
    [SerializeField] private int ProjectilePlayerOnlyEnemySummonCount;
    [SerializeField] private int ProjectilePlayerOnlyTwoEnemySummonCount;

    [Header("프리팹")]
    [SerializeField] private GameObject EnemyPrefab_Spider;
    [SerializeField] private GameObject EnemyPrefab_Projectile;
    [SerializeField] private GameObject EnemyPrefab_AreaDelayed;
    [SerializeField] private GameObject EnemyPrefab_WagonOnly;
    [SerializeField] private GameObject EnemyPrefab_PlayerOnlyOne;
    [SerializeField] private GameObject EnemyPrefab_PlayerOnlyTwo;

    private const string MELEE_ENEMY_ID = "Spider";
    private const string PROJECTILE_ENEMY_ID = "EvilMageRed";
    private const string AREA_DELAYED_ENEMY_ID = "MonsterPlant";
    private const string MELEE_WAGONONLY_ENEMY_ID = "RatAssassin";
    private const string PROJECTILE_PLAYERONLY_ENEMY_ID = "EvilMagePurple";
    private const string PROJECTILE_PLAYERONLY_ENEMY_TWO_ID = "Bat";

    private DataTable _dataTable;

    private void Awake()
    {
        _dataTable = new DataTable();
        _dataTable.LoadAllData();
    }

    private void Update()
    {
        _time += Time.deltaTime;

        if (_time >= SpawnTime)
        {
            _time = 0;

            if (SpawnEnemy)
            {
                SpawnAllEnemies();
            }
        }
    }

    private void SpawnAllEnemies()
    {
        SpawnEnemies(MELEE_ENEMY_ID, EnemyPrefab_Spider, MeleeEnemySummonCount);
        SpawnEnemies(PROJECTILE_ENEMY_ID, EnemyPrefab_Projectile, ProjectileEnemySummonCount);
        SpawnEnemies(AREA_DELAYED_ENEMY_ID, EnemyPrefab_AreaDelayed, AreaDelayedEnemySummonCount);
        SpawnEnemies(MELEE_WAGONONLY_ENEMY_ID, EnemyPrefab_WagonOnly, MeleeWagonOnlyEnemySummonCount);
        SpawnEnemies(PROJECTILE_PLAYERONLY_ENEMY_ID, EnemyPrefab_PlayerOnlyOne, ProjectilePlayerOnlyEnemySummonCount);
        SpawnEnemies(PROJECTILE_PLAYERONLY_ENEMY_TWO_ID, EnemyPrefab_PlayerOnlyTwo, ProjectilePlayerOnlyTwoEnemySummonCount);
    }

    private void SpawnEnemies(string enemyId, GameObject prefab, int count)
    {
        for (int i = 0; i < count; i++)
        {
            SpawnOneEnemy(enemyId, prefab, i);
        }
    }

    private void SpawnOneEnemy(string enemyId, GameObject prefab, int spawnIndex)
    {
        if (prefab == null)
        {
            Debug.LogError($"TestGameObjectManager : {enemyId} 프리팹이 비어있습니다!!");
            return;
        }

        if (_dataTable.EnemyDataTable.TryGetValue(enemyId, out EnemyData enemyData) == false)
        {
            Debug.LogError($"TestGameObjectManager : EnemyData({enemyId})가 없습니다!!");
            return;
        }

        Vector3 spawnPosition = ChoiceTransform(spawnIndex).position;

        EnemyView view = Instantiate(prefab, spawnPosition, Quaternion.identity).GetComponent<EnemyView>();

        EnemyModel model = new EnemyModel(enemyData);
        EnemyViewModel viewModel = new EnemyViewModel(model);

        view.BindViewModel(viewModel);
        view.Init(Wagon, Player);
    }

    private Transform ChoiceTransform(int i)
    {
        switch (i % 4)
        {
            case 0:
                {
                    return SpawnPointOne;
                }
            case 1:
                {
                    return SpawnPointTwo;
                }
            case 2:
                {
                    return SpawnPointThree;
                }
            case 3:
                {
                    return SpawnPointFour;
                }
            default:
                {
                    return null;
                }
        }
    }
}
