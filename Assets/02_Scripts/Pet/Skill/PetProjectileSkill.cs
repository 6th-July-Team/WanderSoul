using System;
using UnityEngine;

public class PetProjectileSkill : IPetActiveSkillExecution
{
    public void Execute(PetSkillUseContext context, float damage, Action OnEndSkill)
    {
        // 투사체는 오브젝트 풀링으로 발사하기
        Debug.Log("펫 엑티브 공격");

        // TODO 공격 모션 끝나면 실행하기
        OnEndSkill?.Invoke();
    }
}
