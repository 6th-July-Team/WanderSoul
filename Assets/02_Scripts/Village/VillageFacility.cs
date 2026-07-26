using UnityEngine;

public enum VillageFacilityType
{
    TownHall,
    MonsterFarm,
    MissionGuild,
    Shop,
    Clinic
}

public class VillageFacility : MonoBehaviour
{
    [SerializeField] private VillageFacilityType _facilityType;

    [Header("건물 정보")]
    [SerializeField] private string _spotDataId;

    private InteractableSpotData _spotData;

    public VillageFacilityType FacilityType => _facilityType;
    public string FacilityName => _spotData?.Name ?? string.Empty;
    public string FacilityDescription => _spotData?.Description ?? string.Empty;

    private void Start()
    {
        _spotData = GameManager.DataTable.GetInteractableSpotData(_spotDataId);

        if (_spotData == null)
        {
            Debug.LogWarning($"{gameObject.name}: {_spotDataId} 데이터를 찾을 수 없습니다.");
        }
    }

}
