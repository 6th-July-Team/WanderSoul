using UnityEngine;

[CreateAssetMenu(fileName = "SOSkillDefinition", menuName = "ScriptableObjects/Player/SOSkillDefinition")]
public class SOSkillDefinition : ScriptableObject
{
    public string Id;

    public float Cooldown;
    public float ManaCost;
    public float BaseDamage;
}
