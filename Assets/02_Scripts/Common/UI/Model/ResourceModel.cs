using UnityEngine;

public class ResourceModel : BaseModel
{
    public override void PropertyChangedOnInit()
    {
        OnPropertyChanged(nameof(Soul));
        OnPropertyChanged(nameof(Money));
    }

    private int _soul;
    public int Soul
    {
        get {  return _soul; }
        set
        {
            if(_soul == value)
            {
                return;
            }
            _soul = value;
            OnPropertyChanged(nameof(Soul));
        }

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
