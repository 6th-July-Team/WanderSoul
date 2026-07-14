using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class VillageClickInput : MonoBehaviour
{

    [SerializeField] private GameObject _shopPanel;

    private void Awake()
    {
        _shopPanel.SetActive(false);
    }

    private void Update()
    {
        if (!Mouse.current.leftButton.wasPressedThisFrame)
        {
            return;
        }

        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());

        if (!Physics.Raycast(ray, out RaycastHit hit))
        {
            return;
        }

        if (!hit.collider.TryGetComponent(out VillageFacility facility))
        {
            return;
        }

        if (facility.FacilityType == VillageFacilityType.Shop)
        {
            _shopPanel.SetActive(true);
        }

        if (facility.FacilityType == VillageFacilityType.MissionGuild)
        {
            Debug.Log("Mission Guild clicked");
        }
    }
}
