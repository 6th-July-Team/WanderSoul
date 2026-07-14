using UnityEngine;

public class PlayerSkill
{
    public PlayerSkillData SkillData { get; private set; }
    public float RemainingCooldTime => _remainingCooldtime;
    public bool IsReady => _remainingCooldtime <= 0f;

    private PlayerStatController _statController;
    private ManaPool _manaPool;
    private readonly IPlayerSkillExecution _execution;
    private float _remainingCooldtime;


    public PlayerSkill(string playerSkillId, IPlayerSkillExecution execution, ManaPool manaPool, PlayerStatController statController)
    {
        SkillData = GameManager.DataTable.GetPlayerSkillData(playerSkillId);
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

    public void CheckSkillRange(PlayerSkillUseContext context)
    {
        if (_execution is ISkillRangeCheckable rangeCheck)
        {
            rangeCheck.CheckSkillRange(context);
        }
    }

    public void HideSkillRange()
    {
        if (_execution is ISkillRangeCheckable rangeCheck)
        {
            rangeCheck.HideSkillRange();
        }
    }

    public bool TryExecuteSkill(PlayerSkillUseContext context)
    {
        if (!IsReady)
            return false;

        if (!_manaPool.TrySpendMana(SkillData.ManaCost))
            return false;

        float skillDamage = SkillData.Power * _statController.GetValue(PlayerStatType.BasicAttackPower);
        _remainingCooldtime = SkillData.Cooldown - _statController.GetValue(PlayerStatType.CooldownReduction);

        _execution.Execute(context, skillDamage);

        return true;
    }
}
