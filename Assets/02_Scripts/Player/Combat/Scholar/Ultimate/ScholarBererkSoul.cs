using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;

public class ScholarBererkSoul : IPlayerSkillExecution
{
    private List<StatType> _statsToApply;

    public void Execute(PlayerSkillUseContext context, PlayerSkillData SkillData, float damage, float coolTimeReductionOfStat)
    {
        // 공속, 이속, 체력 증가
        //펫 컨트롤러 -> 펫 스텟 컨트롤러의 모디파이어 설정

        // TODO(김익환): 어떤 스텟, 어떤 방식으로 적용할지 데이터 들고 오기
        if(null == _statsToApply)
        {
            _statsToApply = new List<StatType>() { /* TODO(김익환): 스텟 작성하기*/ };
        }

        foreach(var statType in _statsToApply)
        {
            var modifire = new StatModifier(statType, StatModifierOperation.AddPercent, 0.5f);

            GameManager.PetParty.AddModifierForAllPet(modifire);
        }

        // TODO: 데이터에서 지속 시간 가져오기
        StartDurationTimer(5f).Forget();

        var viewModel = GameManager.Network.RequestPlayerSkillViewModel();

        float coolTime = context.PlayerSkillModifier.GetValue(SkillData.Id, SkillData.GetSkillSlot()
            , SkillValueType.Cooldown, SkillData.Cooldown) - coolTimeReductionOfStat;


        viewModel.UseSkill(SkillData.GetSkillSlot(), coolTime);
    }

    private async UniTaskVoid StartDurationTimer(float duration)
    {
        await UniTask.Delay(TimeSpan.FromSeconds(duration));

        foreach (var statType in _statsToApply)
        {
            GameManager.PetParty.RemoveModifierForAllPet(statType);
        }
    }
}
