using System;
using UnityEngine;

public class PetActiveSkill
{
    public PetActiveSkillData SkillData { get; private set; }

    public TargetType TargetType => SkillData.GetTargetType();
    public float CastRange => SkillData.CastRange;
    public float RemainingCooldTime => _remainingCooldtime;
    public bool IsReady => _remainingCooldtime <= 0f;


    private PetStatController _statController;
    private readonly IPetActiveSkillExecution _execution;
    private float _remainingCooldtime;


    public PetActiveSkill(PetActiveSkillData skillData, IPetActiveSkillExecution execution, PetStatController statController)
    {
        SkillData = skillData;
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

        _remainingCooldtime = SkillData.Cooldown - _statController.GetValue(StatType.CooldownReduction);

        _execution.Execute(context, onEndSkill);

        return true;
    }

    public bool CanUse(ITargetable target)
    {
        return TargetType switch
        {
            TargetType.Enemy => target != null && target.IsAlive && target.EntityType == EntityType.Enemy,

            TargetType.Player => true,
            TargetType.Pet => true,

            _ => false
        };
    }
}