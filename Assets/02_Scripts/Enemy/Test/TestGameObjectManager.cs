using Cysharp.Threading.Tasks;
using Unity.VisualScripting;
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

    private GameObject _prefab_meleeEnemy;
    private GameObject _prefab_projectileEnemy;

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

        var (prefab_meleeEnemy, prefab_projectileEnemy) = await UniTask.WhenAll
            (
            GameManager.Resource.LoadAsset<GameObject>(enemyData.PrefabAddress),
            GameManager.Resource.LoadAsset<GameObject>(enemyDataTwo.PrefabAddress)
            );

        _prefab_meleeEnemy = prefab_meleeEnemy;
        _prefab_projectileEnemy = prefab_projectileEnemy;
    }

    private void InstantiateEnemy()
    {
        for (int i = 0; i < 4; i++)
        {
            InstantiateMeleeEnemy(i);
        }

        for (int i = 0; i < 4; i++)
        {
            InstantiateProjectileEnemy(i);
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

    private Vector3 PrevPosition;
    private Vector3 CurrentPosition;

    private void AA()
    {
        Vector3 targetPosition = new Vector3(this.transform.position.x, this.transform.position.y, (this.transform.position.z + 10));



    }
}
