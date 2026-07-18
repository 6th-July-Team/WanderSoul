using Cysharp.Threading.Tasks;
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

    private GameObject _prefab_meleeEnemy;
    private GameObject _prefab_projectileEnemy;
    private GameObject _prefab_areaDelayedEnemy;

    private void Start()
    {
        StartSetting().Forget();
    }

    private async UniTaskVoid StartSetting()
    {
        await LoadAsset();
        InstantiateEnemy();
    }

    private async UniTask LoadAsset()
    {
        GameManager.DataTable.EnemyDataTable.TryGetValue("Test", out EnemyData enemyData);
        GameManager.DataTable.EnemyDataTable.TryGetValue("Test_01", out EnemyData enemyDataTwo);
        GameManager.DataTable.EnemyDataTable.TryGetValue("Test_02", out EnemyData enemyDataThree);

        var (prefab_meleeEnemy, prefab_projectileEnemy, prefab_areaDelayedEnemy) = await UniTask.WhenAll
            (
            GameManager.Resource.LoadAsset<GameObject>(enemyData.PrefabAddress),
            GameManager.Resource.LoadAsset<GameObject>(enemyDataTwo.PrefabAddress),
            GameManager.Resource.LoadAsset<GameObject>(enemyDataThree.PrefabAddress)
            );

        _prefab_meleeEnemy = prefab_meleeEnemy;
        _prefab_projectileEnemy = prefab_projectileEnemy;
        _prefab_areaDelayedEnemy = prefab_areaDelayedEnemy;
    }

    private void InstantiateEnemy()
    {
        for (int i = 0; i < MeleeEnemySummonCount; i++)
        {
            InstantiateMeleeEnemy(i);
        }

        for (int i = 0; i < ProjectileEnemySummonCount; i++)
        {
            InstantiateProjectileEnemy(i);
        }

        for (int i = 0 ; i < AreaDelayedEnemySummonCount; i++)
        {
            InstantiateAreaDelayedEnemy(i);
        }
    }

    private void InstantiateMeleeEnemy(int i)
    {
        GameObject enemy = Instantiate(_prefab_meleeEnemy, ChoiceTransform(i));


        EnemyView view = enemy.GetComponent<EnemyView>();

        GameManager.DataTable.EnemyDataTable.TryGetValue("Test", out EnemyData enemyData);

        if (enemyData == null)
        {
            Debug.LogError("Test이 없습니다");
        }

        EnemyModel model = new EnemyModel(enemyData);
        EnemyViewModel viewModel = new EnemyViewModel(model);

        view.BindViewModel(viewModel);
        view.Init(Wagon, Player);
    }

    private void InstantiateProjectileEnemy(int i)
    {
        GameObject enemy = Instantiate(_prefab_projectileEnemy, ChoiceTransform(i));

        EnemyView view = enemy.GetComponent<EnemyView>();

        GameManager.DataTable.EnemyDataTable.TryGetValue("Test_01", out EnemyData enemyData);

        if (enemyData == null)
        {
            Debug.LogError("Test_01이 없습니다");
        }

        EnemyModel model = new EnemyModel(enemyData);
        EnemyViewModel viewModel = new EnemyViewModel(model);

        view.BindViewModel(viewModel);
        view.Init(Wagon, Player);
    }

    private void InstantiateAreaDelayedEnemy(int i)
    {
        GameObject enemy = Instantiate(_prefab_areaDelayedEnemy, ChoiceTransform(i));

        EnemyView view = enemy.GetComponent<EnemyView>();

        GameManager.DataTable.EnemyDataTable.TryGetValue("Test_02", out EnemyData enemyData);

        if (enemyData == null)
        {
            Debug.LogError("Test_02이 없습니다");
        }

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
