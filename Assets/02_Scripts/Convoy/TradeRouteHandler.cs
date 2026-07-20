using UnityEngine;
using UnityEngine.Splines;

public class TradeRouteHandler : MonoBehaviour
{
    public SplineContainer SplineContainer { get; private set; }

    private void Awake()
    {
        SplineContainer = GetComponentInChildren<SplineContainer>();
    }
}
