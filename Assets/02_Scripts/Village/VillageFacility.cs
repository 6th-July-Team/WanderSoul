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
    [SerializeField] private string _facilityName;

    [TextArea]
    [SerializeField] private string _facilityDescription;

    [SerializeField] private bool _isAvailable = true;

    public VillageFacilityType FacilityType => _facilityType;
    public string FacilityName => _facilityName;
    public string FacilityDescription => _facilityDescription;
    public bool IsAvailable => _isAvailable;

}
