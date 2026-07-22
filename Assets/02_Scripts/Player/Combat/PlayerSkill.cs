using UnityEngine;

public class PlayerSkill
{
    public PlayerSkillData SkillData { get; private set; }
    public float RemainingCooldTime => _remainingCooldtime;
    public bool IsReady => _remainingCooldtime <= 0f;

    private PlayerStatController _statController;
    private PlayerSkillModifier _skillModifier;

    private IPlayerSkillExecution _execution;
    private PlayerViewModel _playerViewModel;

    private SkillSlot _slot;
    private float _remainingCooldtime;

    private bool _isInitialized = false;


    public PlayerSkill(PlayerSkillData skillData, IPlayerSkillExecution execution, PlayerViewModel playerViewModel
        , PlayerStatController statController)
    {
        SkillData = skillData;
        _slot = skillData.GetSkillSlot();

        _playerViewModel = playerViewModel;
        _execution = execution;
        _statController = statController;
    }

    public void Init(PlayerSkillModifier skillModifier)
    {
        _skillModifier = skillModifier;

        _isInitialized = true;
    }

    public void Update(float deltaTime)
    {
        if (_remainingCooldtime <= 0f || !_isInitialized)
            return;

        _remainingCooldtime = Mathf.Max(0f, _remainingCooldtime - deltaTime);
    }

    public void CheckSkillRange(PlayerSkillUseContext context)
    {
        if (_execution is ISkillRangeCheckable rangeCheck)
        {
            rangeCheck.CheckSkillRange(context, SkillData);
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

        if (!_playerViewModel.TrySpendMP(SkillData.ManaCost))
            return false;

        float skillDamage = GetSkillDamage() * _statController.GetValue(StatType.AdditionalDamage);

        _remainingCooldtime = GetSkillCooldown() - _statController.GetValue(StatType.CooldownReduction);
        Debug.Log($"{SkillData.Id} 스킬 : 쿨타임 = {_remainingCooldtime}");
        _execution.Execute(context, SkillData, skillDamage);
        
        Debug.Log($"스킬 사용 : {SkillData.Id}");
        return true;
    }

    public float GetSkillDamage()
    {
        return _skillModifier.GetValue(SkillData.Id, _slot, SkillValueType.Power, SkillData.Power);
    }

    public float GetSkillCooldown()
    {
        return _skillModifier.GetValue(SkillData.Id, _slot, SkillValueType.Cooldown, SkillData.Cooldown);
    }
}
