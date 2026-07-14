using System;
using System.Runtime.InteropServices;
using UnityEngine;

public class PetActiveSkill
{
    public SOPetSkillInfo Definition { get; private set; }

    public float CastRange => Definition.CastRange;
    public float RemainingCooldTime => _remainingCooldtime;
    public bool IsReady => _remainingCooldtime <= 0f;

    private PetStatController _statController;
    private readonly IPetActiveSkillExecution _execution;
    private float _remainingCooldtime;


    public PetActiveSkill(SOPetSkillInfo definition, IPetActiveSkillExecution execution, PetStatController statController)
    {
        Definition = definition;
        _execution = execution;
        _statController = statController;
    }

    public void Update(float deltaTime)
    {
        if (_remainingCooldtime <= 0f)
            return;

        _remainingCooldtime = Mathf.Max(0f, _remainingCooldtime - deltaTime);
    }

    public bool TryExecute(PetSkillUseContext context, Action onEndSkill)
    {
        if (!IsReady)
            return false;

        float skillDamage = Definition.BaseDamage * _statController.GetValue(PetStatType.BasicPower);
        _remainingCooldtime = Definition.Cooldown - _statController.GetValue(PetStatType.CooldownReduction);

        _execution.Execute(context, skillDamage, onEndSkill);

        return true;
    }

    public bool CanTarget(ITargetable target)
    {
        return target != null && target.IsAlive;
    }
}