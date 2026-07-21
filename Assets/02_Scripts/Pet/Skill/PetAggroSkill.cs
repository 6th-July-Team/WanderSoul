using System;
using UnityEngine;

public class PetAggroSkill : IPetActiveSkillExecution
{
    private Collider[] _targets = new Collider[32];

    private StatusEffectMaker _effectMaker;

    private IStatusEffectReceiver _playerEffectReceiver;
    private IStatusEffectReceiver _statusEffectReceiver;

    public PetAggroSkill(StatusEffectMaker effectMaker, IStatusEffectReceiver playerEffectReceiver, IStatusEffectReceiver statusEffectReceiver)
    {
        _effectMaker = effectMaker;
        _playerEffectReceiver = playerEffectReceiver;
        _statusEffectReceiver = statusEffectReceiver;
    }

    public void Execute(PetSkillUseContext context, Action OnEndSkill)
    {
        // 펫 원형 범위 적에게
        // 도발을 걸고

        SearchUtil.FindTargetSphere(context.PetPos, context.PetActiveSkillData.Radius
            , LayerMask.GetMask("Enemy"), _targets);

        foreach(var target in _targets)
        {
            if (target == null) 
                continue;

            var enemy = target.GetComponent<IEnemy>();

            if (enemy == null) 
                continue;

            enemy.ApplyTaunt(context.IPet, context.PetActiveSkillData.Duration);
        }


        // 도발 상태에선 펫 방어력이 50 증가.
        var data = GameManager.DataTable.GetStatusEffectData(context.PetActiveSkillData.StatusEffectId);
        SkillModifierData skillModifierData = GameManager.DataTable.GetSkillModifierData(data.SkillModifierId);

        StatusEffectInstance effect = _effectMaker.Create(_playerEffectReceiver, data, skillModifierData);


        _statusEffectReceiver.StatusEffects.Apply(effect);
    }
}
