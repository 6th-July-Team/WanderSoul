using UnityEngine;

public interface ISkillRangeCheckable
{
    void CheckSkillRange(PlayerSkillUseContext context);
    void HideSkillRange();
}
