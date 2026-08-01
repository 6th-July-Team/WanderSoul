using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class VillageClickInput : MonoBehaviour
{
    [Header("건물 호버 UI")]
    [SerializeField] private RectTransform _facilityHoverPanel;
    [SerializeField] private TMP_Text _facilityInfoText;
    [SerializeField] private Vector2 _facilityHoverOffset = new Vector2(0f, 80f);
    [SerializeField] private float _topUiReservedHeight = 90f;

    private VillageFacility _hoveredFacility;

    private void Awake()
    {
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

        CloseAllPanels();

        if (facility.FacilityType == VillageFacilityType.MissionGuild)
        {
            GameManager.UI.OpenQuestBoardUI();
        }

        if (facility.FacilityType == VillageFacilityType.TownHall)
        {
            GameManager.Instance.OpenTownHall();
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
            _facilityInfoText.text = $"</b>{facility.FacilityName}</b>({facility.FacilityDescription})";
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

    public void CloseAllPanels()
    {
        GameManager.UI.CloseAllQuestUI();
        GameManager.UI.CloseUI(UIType.TownHallUIView);
    }
}
