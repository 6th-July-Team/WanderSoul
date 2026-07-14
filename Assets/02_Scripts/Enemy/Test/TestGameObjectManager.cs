using UnityEngine;

public class TestGameObjectManager : MonoBehaviour
{
    [SerializeField] private Transform SpawnPointOne;
    [SerializeField] private Transform SpawnPointTwo;
    [SerializeField] private Transform SpawnPointThree;
    [SerializeField] private Transform SpawnPointFour;

    [SerializeField] private GameObject Prefab_Enemy;
    [SerializeField] private GameObject Player;
    [SerializeField] private GameObject Caravan;
    [SerializeField] private GameObject Pet;
    [SerializeField] private UI UI;

    private void Start()
    {
        for (int i = 0; i < 100; i++)
        {
            InstantiateEnemy(i);
        }

        CreateButtonActionClass();
    }

    private void InstantiateEnemy(int i)
    {
        GameObject enemy = Instantiate(Prefab_Enemy, ChoiceTransform(i));

        MoveableEnemyView view = enemy.GetComponent<MoveableEnemyView>();

        GameManager.DataTable.EnemyDataTable.TryGetValue("Test", out EnemyData enemyData);

        if (enemyData == null)
        {
            Debug.LogError("Test.Json이 없습니다");
        }

        MoveableEnemyModel model = new MoveableEnemyModel(enemyData);
        MoveableEnemyViewModel viewModel = new MoveableEnemyViewModel(model);

        view.BindViewModel(viewModel);
        view.Init(Caravan, Player);
    }

    private void CreateButtonActionClass()
    {
        ButtonAction buttonAction = new(UI, Player, Caravan, Pet);
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
