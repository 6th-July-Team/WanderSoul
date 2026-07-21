using UnityEngine;

public class NetworkEnemyService
{
    public EnemyViewModel CreateEnemyViewModel(string enemyId)
    {
        if (GameManager.DataTable.EnemyDataTable.TryGetValue(enemyId, out EnemyData enemyData) == false)
        {
            Debug.LogError($"TestGameObjectManager : EnemyData({enemyId})가 없습니다!!");
            return null;
        }

        EnemyModel model = new EnemyModel(enemyData);
        var viewModel = new EnemyViewModel(model);

        return viewModel;
    }
}
