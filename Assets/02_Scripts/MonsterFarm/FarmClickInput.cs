using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class FarmClickInput : MonoBehaviour
{

    [SerializeField] private GameObject _summonPanel;

    private void Awake()
    {
        _summonPanel.SetActive(false);
    }

    private void Update()
    {
        if (!Mouse.current.leftButton.wasPressedThisFrame)
        {
            return;
        }

        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());

        if (!Physics.Raycast(ray, out RaycastHit hit))
        {
            return;
        }

        if (!hit.collider.TryGetComponent(out FarmFacility farmFacility))
        {
            return;
        }

        if (farmFacility.FacilityType == FarmFacilityType.SummonCircle)
        {
            _summonPanel.SetActive(true);
        }
    }
}
