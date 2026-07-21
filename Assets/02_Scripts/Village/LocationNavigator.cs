using UnityEngine;
using System.Collections.Generic;

public class LocationNavigator : MonoBehaviour
{
    [SerializeField] private GameObject _startLocation;

    private readonly Stack<GameObject> _locationHistory = new();
    private GameObject _currentLocation;

    private void Awake()
    {
        _currentLocation = _startLocation;
        _currentLocation.SetActive(true);
    }

    public void Enter(GameObject nextlocation)
    {
        if (nextlocation == null || nextlocation == _currentLocation)
        {
            return;
        }

        _locationHistory.Push(_currentLocation);

        _currentLocation.SetActive(false);
        nextlocation.SetActive(true);

        _currentLocation = nextlocation;
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
