using System.Collections.Generic;
using UnityEngine;

public class LocationNavigator : MonoBehaviour
{
    [SerializeField] private GameObject _startLocationPrefab;
    [SerializeField] private GameObject _monsterFarmPrefab;

    private readonly Stack<GameObject> _locationHistory = new();

    private GameObject _startLocation;
    private GameObject _monsterFarmRoot;
    private GameObject _currentLocation;

    public bool IsInMonsterFarm => _currentLocation == _monsterFarmRoot;

    private void Start()
    {
        if (_startLocationPrefab == null || _monsterFarmPrefab == null)
        {
            Debug.LogError("LocationNavigator에 장소 프리팹이 연결되지 않았습니다.");
            return;
        }

        _startLocation = Instantiate(_startLocationPrefab, transform);
        _monsterFarmRoot = Instantiate(_monsterFarmPrefab, transform);

        _currentLocation = _startLocation;
        _startLocation.SetActive(true);
        _monsterFarmRoot.SetActive(false);
    }

    public void ToggleMonsterFarm()
    {
        if (IsInMonsterFarm)
        {
            GoBack();
            return;
        }

        EnterMonsterFarm();
    }

    public void EnterMonsterFarm()
    {
        Enter(_monsterFarmRoot);
    }

    public void Enter(GameObject nextLocation)
    {
        if (nextLocation == null || nextLocation == _currentLocation)
        {
            return;
        }

        _locationHistory.Push(_currentLocation);

        _currentLocation.SetActive(false);
        nextLocation.SetActive(true);

        _currentLocation = nextLocation;
    }

    public void GoBack()
    {
        if (_locationHistory.Count == 0)
        {
            return;
        }

        _currentLocation.SetActive(false);
        _currentLocation = _locationHistory.Pop();
        _currentLocation.SetActive(true);
    }
}