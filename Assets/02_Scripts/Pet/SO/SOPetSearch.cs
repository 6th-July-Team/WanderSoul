using UnityEngine;


[CreateAssetMenu(fileName = "SOPetSearch", menuName = "ScriptableObjects/Pet/SOPetSearch")]
public class SOPetSearch : ScriptableObject
{
    public float RangeWhenFollowPlayer;
    public float RangeWhenGuardCart;
    public float RangeWhenAggressive;
} 