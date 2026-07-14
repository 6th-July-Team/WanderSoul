using UnityEngine;

public class ScholarFireArrow : IElementArrowVariant
{
    public void Fire(PlayerSkillUseContext context, float damage)
    {
        // 화염 화살을 발사해서 화염 피해를 입힌다.
        // 대상에 닿으면 폭발하여 넓은 범위의 적에게 피해를 준다.
        Debug.Log("학자 기본 스킬: 화염 화살 발사!");
    }
}
