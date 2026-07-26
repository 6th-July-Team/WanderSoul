using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class FarmClickInput : MonoBehaviour
{

    [SerializeField] private GameObject _summonPanel;
    [SerializeField] private MonsterCorral _monsterCorral;
    [SerializeField] private CorralPanel _corralPanel;
    [SerializeField] private ManagementPanel _managementPanel;

    private void Awake()
    {
        _summonPanel.SetActive(false);
        _corralPanel.Close();
        _managementPanel.Close();
    }

    private void Update()
    {
        if (!Mouse.current.leftButton.wasPressedThisFrame)
        {
            return;
        }

        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
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

        CloseAllPanels();

        if (farmFacility.FacilityType == FarmFacilityType.SummonCircle)
        {
            _summonPanel.SetActive(true);
        }

        if (farmFacility.FacilityType == FarmFacilityType.Corral)
        {
            _corralPanel.Open(_monsterCorral);
        }

        if (farmFacility.FacilityType == FarmFacilityType.Storage)
        {
            _managementPanel.Open();
        }
    }

    public void CloseAllPanels()
    {
        _summonPanel.SetActive(false);
        _corralPanel.Close();
        _managementPanel.Close();
    }
}
