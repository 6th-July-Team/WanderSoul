using UnityEngine;

public class PlayerSkillViewModel : BaseViewModel<PlayerSkillModel>
{

    public PlayerSkillViewModel(PlayerSkillModel model) : base(model)
    {
    }

    public void UseSkill(SkillSlot slot, float coolTime)
    {
        _model.SkillCoolTimes[slot] = coolTime;
    }

    public float GetSkillCoolTime(SkillSlot slot)
    {
        return _model.SkillCoolTimes[slot];
    }

    /// <summary>
    /// 첫 등록의 경우 쿨타임이 0
    /// </summary>
    public void SetSkill(SkillSlot slot, float coolTime = 0f)
    {
        _model.SetSkillCoolTime(slot, coolTime);
    }

    public void Update(SkillSlot slot, float deltaTime)
    {
        _model.SetSkillCoolTime(slot, Mathf.Max(0f, _model.SkillCoolTimes[slot] - deltaTime));
    }
}
