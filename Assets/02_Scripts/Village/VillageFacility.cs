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

    public VillageFacilityType FacilityType => _facilityType;
}
