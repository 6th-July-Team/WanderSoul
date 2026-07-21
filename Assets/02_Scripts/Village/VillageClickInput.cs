using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class VillageClickInput : MonoBehaviour
{

    [SerializeField] private GameObject _shopPanel;
    [SerializeField] private GameObject _missionPanel;
    [SerializeField] private GameObject _townHallPanel;
    [SerializeField] private LocationNavigator _locationNavigator;
    [SerializeField] private GameObject _monsterFarmRoot;

    private void Awake()
    {
        _shopPanel.SetActive(false);
        _missionPanel.SetActive(false);
        _townHallPanel.SetActive(false);
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

        if (facility.FacilityType == VillageFacilityType.MonsterFarm)
        {
            _locationNavigator.Enter(_monsterFarmRoot);
            return;
        }

        if (facility.FacilityType == VillageFacilityType.Shop)
        {
            _shopPanel.SetActive(true);
            _missionPanel.SetActive(false);
            _townHallPanel.SetActive(false);
        }

        if (facility.FacilityType == VillageFacilityType.MissionGuild)
        {
            _missionPanel.SetActive(true);
            _shopPanel.SetActive(false);
            _townHallPanel.SetActive(false);
        }

        if (facility.FacilityType == VillageFacilityType.TownHall)
        {
            _townHallPanel.SetActive(true);
            _shopPanel.SetActive(false);
            _missionPanel.SetActive(false);
        }
    }
}
