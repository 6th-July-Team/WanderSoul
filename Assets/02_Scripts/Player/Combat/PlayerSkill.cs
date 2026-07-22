using UnityEngine;

public class PlayerSkill
{
    public PlayerSkillData SkillData { get; private set; }
    public float RemainingCooldTime => _skillViewModel.GetSkillCoolTime(_slot);
    public bool IsReady => RemainingCooldTime <= 0f;

    private PlayerStatController _statController;
    private PlayerSkillModifier _skillModifier;

    private IPlayerSkillExecution _execution;

    private PlayerViewModel _playerViewModel;
    private PlayerSkillViewModel _skillViewModel;

    private SkillSlot _slot;

    private bool _isInitialized = false;


    public PlayerSkill(PlayerSkillData skillData, IPlayerSkillExecution execution, PlayerViewModel playerViewModel
        , PlayerStatController statController)
    {
        SkillData = skillData;
        _slot = skillData.GetSkillSlot();

        _playerViewModel = playerViewModel;
        _execution = execution;
        _statController = statController;

        _skillViewModel = GameManager.Network.RequestPlayerSkillViewModel();
        _skillViewModel.SetSkill(_slot, 0f);
    }

    public void Init(PlayerSkillModifier skillModifier)
    {
        _skillModifier = skillModifier;

        _isInitialized = true;
    }

    public void Update(float deltaTime)
    {
        if (IsReady || !_isInitialized)
            return;

        _skillViewModel.Update(_slot, deltaTime);
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

        float coolTimeReductionOfStat = _statController.GetValue(StatType.CooldownReduction);

        _execution.Execute(context, SkillData, skillDamage, coolTimeReductionOfStat);
        
        Debug.Log($"스킬 사용 : {SkillData.Id}");
        return true;
    }

    public float GetSkillDamage()
    {
        return _skillModifier.GetValue(SkillData.Id, _slot, SkillValueType.Power, SkillData.Power);
    }
}
