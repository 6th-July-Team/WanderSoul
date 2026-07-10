using UnityEngine;
using UnityEngine.InputSystem;

public class FarmClickInput : MonoBehaviour
{

    [SerializeField] private GameObject _summonPanel;
    [SerializeField] private MonsterCorral _monsterCorral;
    [SerializeField] private CorralPanel _corralPanel;

    private void Awake()
    {
        _summonPanel.SetActive(false);
        _corralPanel.Close();
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

        if (farmFacility.FacilityType == FarmFacilityType.Corral)
        {
            _corralPanel.Open(_monsterCorral);
        }
    }
}
