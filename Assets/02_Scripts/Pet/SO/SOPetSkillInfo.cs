using UnityEngine;

[CreateAssetMenu(fileName = "SOPetSkillInfo", menuName = "ScriptableObjects/Pet/PetSkillInfo")]
public class SOPetSkillInfo : ScriptableObject
{
    public string Id;
    public string Name;
    public float BaseDamage;
    public float Cooldown;
    public float CastRange;
    public float Power;
    public float Duration;
    public float Radius;

    public PetSkillSlot SkillSlot;

    public string Effect;
}
