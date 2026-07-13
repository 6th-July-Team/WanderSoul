using UnityEngine;

public class ItemSlotModel : BaseModel
{
    private long _itemUniqueId;
    public long ItemUniqueId
    {
        get { return _itemUniqueId; }
        set
        {
            if (_itemUniqueId == value)
            {
                return;
            }
            _itemUniqueId = value;
            OnPropertyChanged(nameof(_itemUniqueId));
        }
    }

    private string _itemDataId;
    public string ItemDataId
    {
        get {return _itemDataId;}
        set
        {
            if (_itemDataId == value)
            {
                return;
            }
            _itemDataId = value;
            OnPropertyChanged(nameof(_itemDataId));
        }
    }

    private int _stackCount;
    public int StackCount
    {
        get { return _stackCount; }
        set
        {
            if (_stackCount == value)
            {
                return;
            }
            _stackCount = value;
            OnPropertyChanged(nameof(_stackCount));
        }
    }





    public override void PropertyChangedOnInit()
    {
        OnPropertyChanged(nameof(_itemUniqueId));
        OnPropertyChanged(nameof(_itemDataId));
        OnPropertyChanged(nameof(_stackCount));
    }
}
