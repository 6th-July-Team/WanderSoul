

public class PlayerOutGameModel : BaseModel
{
    public override void PropertyChangedOnInit()
    {
        OnPropertyChanged(nameof(Soul));
        OnPropertyChanged(nameof(Gold));
        OnPropertyChanged(nameof(Exp));
        OnPropertyChanged(nameof(Reputation));
    }

    private int _soul;
    public int Soul
    {
        get => _soul;
        set
        {
            if (_soul == value)
                return;
            _soul = value;
            OnPropertyChanged(nameof(Soul));
        }
    }

    private int _gold;
    public int Gold
    {
        get => _gold;
        set
        {
            if (_gold == value)
                return;
            _gold = value;
            OnPropertyChanged(nameof(Gold));
        }
    }

    private float _exp;
    public float Exp
    {
        get => _exp;
        set
        {
            if (_exp == value)
                return;
            _exp = value;
            OnPropertyChanged(nameof(Exp));
        }
    }

    private int _reputation;
    public int Reputation
    {
        get => _reputation;
        set
        {
            if (_reputation == value)
                return;
            _reputation = value;
            OnPropertyChanged(nameof(Reputation));
        }
    }
}
