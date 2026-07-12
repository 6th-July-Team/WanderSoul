using UnityEngine;

public class PlayerSkill
{
    public SOSkillDefinition Definition { get; private set; }
    public float RemainingCooldown => _remainingCooldown;
    public bool IsReady => _remainingCooldown <= 0f;

    private PlayerStatController _statController;
    private ManaPool _manaPool;
    private readonly ISkillExecution _execution;
    private float _remainingCooldown;


    public PlayerSkill(SOSkillDefinition definition, ISkillExecution execution, ManaPool manaPool, PlayerStatController statController)
    {
        Definition = definition;
        _execution = execution;
        _manaPool = manaPool;
        _statController = statController;
    }

    public void Update(float deltaTime)
    {
        if (_remainingCooldown <= 0f)
            return;

        _remainingCooldown = Mathf.Max(0f, _remainingCooldown - deltaTime);
    }

    public void TryExecuteSkill(SkillUseContext context)
    {
        if (!IsReady)
            return;

        if (!_manaPool.TrySpendMana(Definition.ManaCost))
            return;

        float skillDamage = Definition.BaseDamage * _statController.GetValue(PlayerStatType.BasicAttackPower);

        _execution.Execute(context, skillDamage);

        _remainingCooldown = Definition.Cooldown - _statController.GetValue(PlayerStatType.CooldownReduction);
    }
}
