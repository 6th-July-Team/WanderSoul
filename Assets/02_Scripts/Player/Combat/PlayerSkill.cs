using UnityEngine;

public class PlayerSkill
{
    public SOSkillDefinition Definition { get; private set; }
    public float RemainingCooldTime => _remainingCooldtime;
    public bool IsReady => _remainingCooldtime <= 0f;

    private PlayerStatController _statController;
    private ManaPool _manaPool;
    private readonly IPlayerSkillExecution _execution;
    private float _remainingCooldtime;


    public PlayerSkill(SOSkillDefinition definition, IPlayerSkillExecution execution, ManaPool manaPool, PlayerStatController statController)
    {
        Definition = definition;
        _execution = execution;
        _manaPool = manaPool;
        _statController = statController;
    }

    public void Update(float deltaTime)
    {
        if (_remainingCooldtime <= 0f)
            return;

        _remainingCooldtime = Mathf.Max(0f, _remainingCooldtime - deltaTime);
    }

    public void TryExecuteSkill(PlayerSkillUseContext context)
    {
        if (!IsReady)
            return;

        if (!_manaPool.TrySpendMana(Definition.ManaCost))
            return;

        float skillDamage = Definition.BaseDamage * _statController.GetValue(PlayerStatType.BasicAttackPower);

        _execution.Execute(context, skillDamage);

        _remainingCooldtime = Definition.Cooldown - _statController.GetValue(PlayerStatType.CooldownReduction);
    }
}
