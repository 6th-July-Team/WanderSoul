using UnityEngine;

public enum FarmFacilityType
{
    Corral,
    SummonCircle,
    Storage
}

public class FarmFacility : MonoBehaviour
{
    [SerializeField] private FarmFacilityType _facilityType;

    public FarmFacilityType FacilityType => _facilityType;
}
