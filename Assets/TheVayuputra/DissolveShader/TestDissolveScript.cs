using TheVayuputra;
using UnityEngine;

public class TestDissolveScript : MonoBehaviour
{
    private DissolveController _dissolve;
    [SerializeField] private bool IsAlive;

    private void Awake()
    {
        _dissolve = this.GetComponent<DissolveController>();
    }

    private void Update()
    {
        switch(IsAlive)
        {
            case true:
                {
                    _dissolve.ReverseDissolve();
                }
                break;
            case false:
                {
                    _dissolve.PlayDissolve();
                }
                break;
        }
    }

}
