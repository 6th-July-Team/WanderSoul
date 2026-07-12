using System.Collections.Generic;
using UnityEngine;

public class PetPassiveController
{
    //private readonly List<IPetPassiveEffect> _effects = new();

    //public void Init(SOPetDefinition definition, PetPassiveContext context)
    //{
    //    _effects.Clear();

    //    foreach (var passive in definition.Passives)
    //    {
    //        IPetPassiveEffect effect = PetPassiveFactory.Create(passive);
    //        effect.Apply(context);
    //        _effects.Add(effect);
    //    }
    //}

    //public void Release(PetPassiveContext context)
    //{
    //    foreach (var effect in _effects)
    //    {
    //        effect.Remove(context);
    //    }

    //    _effects.Clear();
    //}
}


//패시브가 “상시 적용”이면 매 프레임 돌릴 필요 없다.

//전투 시작
//→ Apply()

//전투 종료 / 펫 사망 / 펫 교체
//→ Remove()

//조건부 패시브라면 별도 이벤트 기반으로 처리.

//마차 피격 시
//플레이어 체력 30% 이하 시
//펫 스킬 사용 시
//웨이브 시작 시
