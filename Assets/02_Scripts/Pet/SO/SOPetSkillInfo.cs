using UnityEngine;

[CreateAssetMenu(fileName = "SOPetSkillInfo", menuName = "ScriptableObjects/Pet/PetSkillInfo")]
public class SOPetSkillInfo : ScriptableObject
{
    public string Id;
    public string Name;

    public float Cooldown;
    public float Range;
    public float Power;
    public float Duration;
    public float Radius;

    public EPetSkillType SkillSlot;
    public EPetSkillTrigger TriggerType;

    public ESkillBehavior BehaviorType;
}
