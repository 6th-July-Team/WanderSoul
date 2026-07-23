using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkillModel : BaseModel, ContainerPropertyChanged<SkillSlot>
{
    public event Action<string, ContainerEventType, SkillSlot> ContainerPropertyChanged;

    public override void PropertyChangedOnInit()
    {
        OnPropertyChanged(nameof(SkillCoolTimes));
    }

    private Dictionary<SkillSlot, float> _skillCoolTimes = new();
    public Dictionary<SkillSlot, float> SkillCoolTimes { get => _skillCoolTimes; }

    public void SetSkillCoolTime(SkillSlot slot, float value)
    {
        if (_skillCoolTimes.TryGetValue(slot, out float current) && Mathf.Approximately(current, value))
        {
            return;
        }

        _skillCoolTimes[slot] = value;
        ContainerPropertyChanged?.Invoke(nameof(SkillCoolTimes), ContainerEventType.Update, slot);
    }
}
