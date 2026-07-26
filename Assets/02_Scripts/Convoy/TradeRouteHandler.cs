using UnityEngine;
using UnityEngine.Splines;

public class TradeRouteHandler : MonoBehaviour
{
    public SplineContainer SplineContainer { get; private set; }
    [SerializeField] private Transform _playerSpawnPosition;

    public Transform PlayerSpawnPosition => _playerSpawnPosition;

    private void Awake()
    {
        SplineContainer = GetComponentInChildren<SplineContainer>();
    }
}
