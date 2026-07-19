using System;
using UnityEngine;

public class PetWindBlessingSkill : IPetActiveSkillExecution
{
    private StatusEffectMaker _effectMaker;
    private StatusEffectData _effectData;


    private IStatusEffectReceiver _playerEffectReceiver;
    private IHealable _playerHealable;

    public PetWindBlessingSkill(StatusEffectMaker effectMaker
        , StatusEffectData effectData
        , IStatusEffectReceiver playerEffectReceiver, IHealable playerHealable)
    {
        _effectMaker = effectMaker;
        _effectData = effectData;
        _playerEffectReceiver = playerEffectReceiver;
        _playerHealable = playerHealable;
    }

    public void Execute(PetSkillUseContext context, Action onEndSkill)
    {
        Debug.Log($"{GetType()}: 스킬 발동");

        bool wasHealthFull = _playerHealable.IsHealthFull;

        if (wasHealthFull)
        {
            ApplyCooldownBuff();
        }
        else
        {
            _playerHealable.Heal(context.PetActiveSkillData.Power);
        }

        onEndSkill?.Invoke();
    }

    private void ApplyCooldownBuff()
    {
        SkillModifierData skillModifierData = GameManager.DataTable.GetSkillModifierData(_effectData.SkillModifierId);
        StatusEffectInstance effect = _effectMaker.Create(_playerEffectReceiver, _effectData, skillModifierData);

        if (effect == null)
        {
            Logger.LogWarning($"{GetType()}: 쿨다운 버프 생성 실패");
            return;
        }


        _playerEffectReceiver.StatusEffects.Apply(effect);
    }
}
