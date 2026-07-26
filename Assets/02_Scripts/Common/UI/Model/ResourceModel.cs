using UnityEngine;

public class ResourceModel : BaseModel
{
    public override void PropertyChangedOnInit()
    {
        OnPropertyChanged(nameof(Money));
    }

    private int _money;
    public int Money
    {
        get { return _money; }
        set
        {
            if (_money == value)
            {
                return;
            }

            _money = value;
            OnPropertyChanged(nameof(Money));

        }
    }
}
