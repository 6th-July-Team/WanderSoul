using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class VillageClickInput : MonoBehaviour
{

    [SerializeField] private GameObject _missionPanel;
    [SerializeField] private GameObject _townHallPanel;

    [Header("건물 호버 UI")]
    [SerializeField] private RectTransform _facilityHoverPanel;
    [SerializeField] private TMP_Text _facilityInfoText;
    [SerializeField] private Vector2 _facilityHoverOffset = new Vector2(0f, 80f);
    [SerializeField] private float _topUiReservedHeight = 90f;

    private VillageFacility _hoveredFacility;

    private void Awake()
    {
        _missionPanel.SetActive(false);
        _townHallPanel.SetActive(false);
        _facilityHoverPanel.gameObject.SetActive(false);
    }

    private void Update()
    {
        UpdateFacilityHover();

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
            _missionPanel.SetActive(false);
            _townHallPanel.SetActive(false);
        }

        if (facility.FacilityType == VillageFacilityType.MissionGuild)
        {
            _missionPanel.SetActive(false);
            _townHallPanel.SetActive(false);

            GameManager.UI.OpenQuestBoardUI();
        }

        if (facility.FacilityType == VillageFacilityType.TownHall)
        {
            _townHallPanel.SetActive(true);
            _missionPanel.SetActive(false);
        }

        if (facility.FacilityType == VillageFacilityType.Clinic)
        {
            _townHallPanel.SetActive(false);
            _missionPanel.SetActive(false);
        }
    }

    private void UpdateFacilityHover()
    {
        if (Mouse.current == null || Camera.main == null)
        {
            HideFacilityHover();
            return;
        }

        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
        {
            HideFacilityHover();
            return;
        }

        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());

        if (!Physics.Raycast(ray, out RaycastHit hit)
            || !hit.collider.TryGetComponent(out VillageFacility facility))
        {
            HideFacilityHover();
            return;
        }

        if (_hoveredFacility != facility)
        {
            _hoveredFacility = facility;
            _facilityInfoText.text = $"{facility.FacilityName}{facility.FacilityDescription}";
            _facilityHoverPanel.gameObject.SetActive(true);
        }

        Vector3 buildingScreenPosition = Camera.main.WorldToScreenPoint(hit.collider.bounds.center);
        Vector3 hoverPosition = buildingScreenPosition + new Vector3(_facilityHoverOffset.x, _facilityHoverOffset.y, 0f);

        float panelWidth = _facilityHoverPanel.rect.width * _facilityHoverPanel.lossyScale.x;
        float panelHeight = _facilityHoverPanel.rect.height * _facilityHoverPanel.lossyScale.y;

        hoverPosition.x = Mathf.Clamp(hoverPosition.x,panelWidth * _facilityHoverPanel.pivot.x,Screen.width - panelWidth * (1f - _facilityHoverPanel.pivot.x));

        hoverPosition.y = Mathf.Clamp(hoverPosition.y,panelHeight * _facilityHoverPanel.pivot.y,Screen.height - _topUiReservedHeight - panelHeight * (1f - _facilityHoverPanel.pivot.y));

        _facilityHoverPanel.position = hoverPosition;

        _facilityHoverPanel.position = hoverPosition;

    }

    private void HideFacilityHover()
    {
        if (_hoveredFacility == null)
        {
            return;
        }

        _hoveredFacility = null;
        _facilityHoverPanel.gameObject.SetActive(false);
    }
}
