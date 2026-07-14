using UnityEngine;

public class ScholarEarthArrow : IElementArrowVariant
{
    public void Fire(PlayerSkillUseContext context, float damage)
    {
        // 무거운 암석 화살을 발사해서 물리 피해를 입힌다.
        // 투사체 속도와 공격 주기는 느리지만 높은 피해를 주며 적을 뒤로 밀어낸다
        Debug.Log("학자 기본 스킬: 암석 화살 발사!");
    }
}
