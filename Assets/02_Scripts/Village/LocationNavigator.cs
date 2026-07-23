using System.Collections.Generic;
using UnityEngine;

public class LocationNavigator : MonoBehaviour
{
    [SerializeField] private GameObject _startLocation;
    [SerializeField] private GameObject _monsterFarmRoot;

    private readonly Stack<GameObject> _locationHistory = new();
    private GameObject _currentLocation;

    public bool IsInMonsterFarm => _currentLocation == _monsterFarmRoot;

    private void Awake()
    {
        _currentLocation = _startLocation;
        _currentLocation.SetActive(true);
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