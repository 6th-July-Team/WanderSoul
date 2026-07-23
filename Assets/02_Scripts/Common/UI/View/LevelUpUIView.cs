using System;
using System.Collections.Generic;

public class LevelUpUIView : CardSelectUIView
{
    protected override UIType SelfUIType
    {
        get { return UIType.LevelUpUIView; }
    }

    public void SetOptions(List<string> optionIdList, Action<string> onOptionSelected
        , Action onClosed = null)
    {
        SetSlots(optionIdList, onOptionSelected, onClosed);
    }
}
