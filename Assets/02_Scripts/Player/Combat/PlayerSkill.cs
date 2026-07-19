using UnityEngine;

public class PlayerSkill
{
    public PlayerSkillData SkillData { get; private set; }
    public float RemainingCooldTime => _remainingCooldtime;
    public bool IsReady => _remainingCooldtime <= 0f;

    private PlayerStatController _statController;
    private PlayerSkillModifier _skillModifier;

    private IPlayerSkillExecution _execution;
    private ManaPool _manaPool;

    private SkillSlot _slot;
    private float _remainingCooldtime;


    public PlayerSkill(string playerSkillId, IPlayerSkillExecution execution, ManaPool manaPool
        , PlayerStatController statController, PlayerSkillModifier skillModifier)
    {
        SkillData = GameManager.DataTable.GetPlayerSkillData(playerSkillId);

        _manaPool = manaPool;
        _execution = execution;
        _skillModifier = skillModifier;
        _statController = statController;

        _slot = SetSlot(playerSkillId);
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

        if (!_manaPool.TrySpendMana(SkillData.ManaCost))
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

    private SkillSlot SetSlot(string id)
    {
        // TODO(김익환) 주석 해제하기
        string stringSlot = "test_test_basic".Split('_')[2]; //id.Split('_')[2];

        return stringSlot switch
        {
            "basic" => SkillSlot.Basic,
            "special" => SkillSlot.Special,
            "ultimate" => SkillSlot.Ultimate,
            _ => throw new System.Exception($"유효하지 않은 슬롯: {stringSlot}"),
        };
    }
}
