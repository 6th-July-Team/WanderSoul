using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private BoxCollider[] spawnAreas;

    private const string SPAWN_DATA_ID_OFFSET = "";
    private float _convoyProgressTimer = 0f;
    private float _elapsedTime = 0f;

    private void Update()
    {
        //_convoyProgressTimer += GameManager.Time.GameDeltaTime;

        //string spawnDataId = ((int)_convoyProgressTimer + SPAWN_DATA_ID_OFFSET).ToString();

        //if (_elapsedTime >= GameManager.DataTable.GetAutoSpawnData(spawnDataId).SpawnInterval)
        //{
        //    _elapsedTime = 0f;
        //    List<string> enemyIds = GameManager.DataTable.GetAutoSpawnData(spawnDataId).EnemyIds;
        //    SpawnEnemy(enemyIds);
        //}
        //else
        //{
        //    _elapsedTime += GameManager.Time.GameDeltaTime;
        //}
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

            // TODO(김익환): 몬스터 초기화 함수 생성시 아래 로직 주석 해제
            //Enemy instanceEnemy =  GameManager.Pool.SpawnFromPool<Enemy>(enemyId, spawnPosition);
            //instanceEnemy.Init(enemyId);
        }
    }
}
