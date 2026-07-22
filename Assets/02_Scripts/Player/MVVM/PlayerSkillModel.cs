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
    public Dictionary<SkillSlot, float> SkillCoolTimes
    {
        get => _skillCoolTimes;
        set
        {
            if (_skillCoolTimes != value)
            {

                _skillCoolTimes = value;
                OnPropertyChanged(nameof(SkillCoolTimes));
                //ContainerPropertyChanged?.Invoke(nameof(SkillCoolTimes), ContainerEventType.Update, SkillSlot);
            }
        }
    }
}
