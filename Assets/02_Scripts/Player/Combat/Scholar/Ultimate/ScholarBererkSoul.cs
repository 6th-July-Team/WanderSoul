using System.Collections.Generic;
using UnityEngine;

public class ScholarBererkSoul : IPlayerSkillExecution
{
    // 캐싱
    private List<StatusEffectData> _effectDatas = new();

    private StatusEffectMaker _effectMaker;
    private IStatusEffectReceiver[] _petEffectReceivers;

    public ScholarBererkSoul(StatusEffectMaker effectMaker, IStatusEffectReceiver[] petEffectReceivers)
    {
        _effectMaker = effectMaker;
        _petEffectReceivers = petEffectReceivers;
    }

    public void Execute(PlayerSkillUseContext context, PlayerSkillData SkillData, float damage, float coolTimeReductionOfStat)
    {
        var effectIds = SkillData.StatusEffectId;

        if (_effectDatas.Count == 0)
        {
            foreach(var effectId in effectIds)
            {
                var statusEffectData = GameManager.DataTable.GetStatusEffectData(effectId);
                if (null == statusEffectData)
                {
                    continue;
                }

                _effectDatas.Add(statusEffectData);
            }
        }

        foreach(var effectData in _effectDatas)
        {
            ApplyBuff(effectData);
        }

        VFXSpawner.SpawnVFX(SkillData.VFXPath, context.Owner.position, Quaternion.identity);
    }

    private void ApplyBuff(StatusEffectData data)
    {
        foreach(var petEffectReceiver in _petEffectReceivers)
        {
            StatusEffectInstance effect = _effectMaker.Create(petEffectReceiver, data);
            petEffectReceiver.StatusEffects.Apply(effect);
        }
    }
}
