using UnityEngine;

public class ScholarWaterArrow : IElementArrowVariant
{
    public void Fire(SkillUseContext context, float damage)
    {
        // 냉기 화살을 발사해서 냉기 피해를 입힌다.
        // 일정 수의 적을 관통하며, 피격된 적의 이동 속도를 잠시 
        Debug.Log("학자 기본 스킬: 냉기 화살 발사!");
    }
}
