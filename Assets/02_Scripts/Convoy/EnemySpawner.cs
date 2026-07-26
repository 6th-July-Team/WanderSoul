using System.Collections.Generic;
using UnityEngine;

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

    private List<string> _enemyPoolId = new();
    private List<string> _enemyIds = new();

    public void Init(PlayerEntity player, Wagon wagon)
    {
        // TODO(김익환): 의뢰 아이디 어디서 가져오지?
        // TODO(김익환): 의뢰 데이터에 접근하는 것을 -> 의뢰를 몰라도 데이터를 알 수 있도록 변경하자.
        List<string> spawnDataIds = GameManager.DataTable.GetQuestData("quest_001").AutoSpawnIds;
        _spawnData = new AutoSpawnData[spawnDataIds.Count];

        for (int i = 0; i < spawnDataIds.Count; i++)
        {
            _spawnData[i] = GameManager.DataTable.GetAutoSpawnData(spawnDataIds[i]);
        }

        _wagonViewModel = GameManager.Network.RequestCreateWagon();

        _player = player;
        _wagon = wagon;

        foreach (var enemyId in _spawnData[0].EnemyIds)
        {
            _enemyPoolId.Add(GameManager.DataTable.GetEnemyData(enemyId).PrefabAddress);
            _enemyIds.Add(enemyId);
        }



        _isInitialized = true;
    }

    public void EncounterSpawn()
    {

    }

    private void Update()
    {
        if (!_isInitialized)
            return;

        _wagonViewModel.Updata(GameManager.Time.GameDeltaTime);

        if (_spawnData[_currentWave].StartTime < _wagonViewModel.GetProgress)
            return;
            
        UpdateWave();

        if (_elapsedTime >= _spawnData[_currentWave].SpawnInterval)
        {
            _elapsedTime = 0f;
            SpawnEnemy();
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

        if (_spawnData[_currentWave].EndTime >= _wagonViewModel.GetTime)
        {
            _currentWave++;

            _enemyPoolId.Clear();
            _enemyIds.Clear();

            foreach (var enemyId in _spawnData[_currentWave].EnemyIds)
            {
                _enemyPoolId.Add(GameManager.DataTable.GetEnemyData(enemyId).PrefabAddress);
                _enemyIds.Add(enemyId);
            }
        }
    }

    private void SpawnEnemy()
    {
        for(int i = 0; i < _enemyIds.Count; i++)
        {
            for(int spawnCount = 0; spawnCount < _spawnData[_currentWave].SpawnBatchCount[i]; spawnCount++)
            {
                BoxCollider spawnArea = spawnAreas[Random.Range(0, spawnAreas.Length)];
                Vector3 spawnPosition = GetRandomPosition(spawnArea);

                var instanceEnemy = GameManager.Pool.SpawnFromPool(_enemyPoolId[i], spawnPosition);

                if (instanceEnemy.TryGetComponent(out EnemyView enemyView))
                {
                    var viewModel = GameManager.Network.CreateEnemyViewModel(_enemyIds[i]);
                    enemyView.BindViewModel(viewModel);
                    enemyView.Init(_wagon.gameObject, _player.gameObject);
                }
            }
        }
    }

    private Vector3 GetRandomPosition(BoxCollider spawnArea)
    {
        Vector3 halfSize = spawnArea.size * 0.5f;

        Vector3 localPosition = spawnArea.center + new Vector3(
            Random.Range(-halfSize.x, halfSize.x),
            0f,
            Random.Range(-halfSize.z, halfSize.z)
        );

        return spawnArea.transform.TransformPoint(localPosition);
    }
}
