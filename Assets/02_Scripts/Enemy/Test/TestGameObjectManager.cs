using UnityEngine;

public class TestGameObjectManager : MonoBehaviour
{
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

    private const string MELEE_ENEMY_ID = "Test";
    private const string PROJECTILE_ENEMY_ID = "Test_01";
    private const string AREA_DELAYED_ENEMY_ID = "Test_02";

    private const string MELEE_WAGONONLY_ENEMY_ID = "Test_03";
    private const string PROJECTILE_PLAYERONLY_ENEMY_ID = "Test_04";
    private const string PROJECTILE_PLAYERONLY_ENEMY_TWO_ID = "Test_06";

    private void Start()
    {
    }

    private void SpawnAllEnemies()
    {
        SpawnEnemies(MELEE_ENEMY_ID, MeleeEnemySummonCount);
        SpawnEnemies(PROJECTILE_ENEMY_ID, ProjectileEnemySummonCount);
        SpawnEnemies(AREA_DELAYED_ENEMY_ID, AreaDelayedEnemySummonCount);
        SpawnEnemies(MELEE_WAGONONLY_ENEMY_ID, MeleeWagonOnlyEnemySummonCount);
        SpawnEnemies(PROJECTILE_PLAYERONLY_ENEMY_ID, ProjectilePlayerOnlyEnemySummonCount);
        SpawnEnemies(PROJECTILE_PLAYERONLY_ENEMY_TWO_ID, ProjectilePlayerOnlyTwoEnemySummonCount);
    }

    private void SpawnEnemies(string enemyId, int count)
    {
        for (int i = 0; i < count; i++)
        {
            SpawnOneEnemy(enemyId, i);
        }
    }

    private void SpawnOneEnemy(string enemyId, int spawnIndex)
    {
        if (GameManager.DataTable.EnemyDataTable.TryGetValue(enemyId, out EnemyData enemyData) == false)
        {
            Debug.LogError($"TestGameObjectManager : EnemyData({enemyId})가 없습니다!!");
            return;
        }

        Vector3 spawnPosition = ChoiceTransform(spawnIndex).position;

        EnemyView view = GameManager.Pool.SpawnFromPool<EnemyView>(enemyData.PrefabAddress, spawnPosition);

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
