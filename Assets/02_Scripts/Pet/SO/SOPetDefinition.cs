using UnityEngine;

[CreateAssetMenu(fileName = "SOPetDefinition", menuName = "ScriptableObjects/Pet/PetDefinition")]
public class SOPetDefinition : ScriptableObject
{
    public string Id;
    public string Name;

    public PetElement Element;

    public SOPetBaseStat BaseStats;
}
