using UnityEngine;

[CreateAssetMenu(fileName = "SOPetDefinition", menuName = "ScriptableObjects/Pet/PetDefinition")]
public class SOPetDefinition : ScriptableObject
{
    public string Id;
    public string Name;

    public EPetElement Element;
    public EPetRole Role;

    public SOPetBaseStat BaseStats;
    public ResistanceInfo ResistanceInfo;
}
