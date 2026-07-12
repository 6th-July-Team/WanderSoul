using UnityEngine;

public class ScholarAirArrow : IElementArrowVariant
{
    public void Fire(SkillUseContext context, float damage)
    {
        // 빠른 번개 화살을 발사해서 전기 피해를 입힌다.
        // 단발의 피해량은 낮지만 공격 주기와 투사체 속도가 빠르다.
        Debug.Log("학자 기본 스킬: 번개 화살 발사!");
    }
}
