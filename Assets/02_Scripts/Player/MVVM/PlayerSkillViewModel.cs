using UnityEngine;

public class PlayerSkillViewModel : BaseViewModel<PlayerSkillModel>
{
    PlayerSkillModel model;

    public PlayerSkillViewModel(PlayerSkillModel model) : base(model)
    {
        this.model = model;
    }

    public void UseSkill(SkillSlot slot)
    {
        model.SkillCoolTimes[slot] = 0;
    }

    public float GetSkillCoolTime(SkillSlot slot)
    {
        return model.SkillCoolTimes[slot];
    }

    public void Update(float deltaTime)
    {

    }
}
