using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Profiling.HierarchyFrameDataView;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private BoxCollider[] spawnAreas;

    private AutoSpawnData[] _spawnData;
    private WagonViewModel _wagonViewModel;

    private float _elapsedTime = 0f;

    private int _currentWave = 0;

    private bool _isInitialized = false;

    private PlayerEntity _player;
    private Wagon _wagon;

    public void Init(PlayerEntity player, Wagon wagon)
    {
        // TODO(김익환): 의뢰 아이디 어디서 가져오지?
        // TODO(김익환): 의뢰 데이터에 접근하는 것을 -> 의뢰를 몰라도 데이터를 알 수 있도록 변경하자.
        List<string> spawnIds = GameManager.DataTable.GetQuestData("quest_001").AutoSpawnIds;
        _spawnData = new AutoSpawnData[spawnIds.Count];

        for (int i = 0; i < spawnIds.Count; i++)
        {
            _spawnData[i] = GameManager.DataTable.GetAutoSpawnData(spawnIds[i]);
        }

        _wagonViewModel = GameManager.Network.RequestCreateWagon();

        _player = player;
        _wagon = wagon;

        _isInitialized = true;
    }

    private void Update()
    {
        if (!_isInitialized || _spawnData[_currentWave].StartTime < _wagonViewModel.GetProgress)
            return;

        UpdateWave();

        if (_elapsedTime >= _spawnData[_currentWave].SpawnInterval)
        {
            _elapsedTime = 0f;
            List<string> enemyIds = _spawnData[_currentWave].EnemyIds;
            SpawnEnemy(enemyIds);
        }
        else
        {
            _elapsedTime += GameManager.Time.GameDeltaTime;
        }
    }

    private void UpdateWave()
    {
        if (_currentWave >= _spawnData.Length - 1)
            return;

        if (_spawnData[_currentWave].EndTime >= _wagonViewModel.GetProgress)
            _currentWave++;
    }

    private void SpawnEnemy(List<string> enemyIds)
    {
        foreach (string enemyId in enemyIds)
        {
            BoxCollider spawnArea = spawnAreas[Random.Range(0, spawnAreas.Length)];

            Vector3 spawnPosition = new Vector3(
                Random.Range(spawnArea.bounds.min.x, spawnArea.bounds.max.x),
                spawnArea.bounds.center.y,
                Random.Range(spawnArea.bounds.min.z, spawnArea.bounds.max.z)
            );

            var instanceEnemy = GameManager.Pool.SpawnFromPool(enemyId, spawnPosition);

            if (instanceEnemy.TryGetComponent(out EnemyView enemyView))
            {
                var viewModel = GameManager.Network.CreateEnemyViewModel(enemyId);
                enemyView.BindViewModel(viewModel);
                enemyView.Init(_player.gameObject, _wagon.gameObject);
            }
        }
    }
}
