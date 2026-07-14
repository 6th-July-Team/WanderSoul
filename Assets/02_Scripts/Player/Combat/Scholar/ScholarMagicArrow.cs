using UnityEngine;

public class ScholarMagicArrow : IElementArrowVariant
{
    public void Fire(PlayerSkillUseContext context, float damage)
    {
        // 마력 화살을 발사해서 무속성 피해를 입힌다.
        // 매 4회 공격마다 더 강한 피해를 주고 적을 관통하는 강화 화살을 발사한다
        Debug.Log("학자 기본 스킬: 마력 화살 발사!");
    }
}
