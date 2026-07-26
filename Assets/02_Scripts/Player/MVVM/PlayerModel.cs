

public class PlayerModel : BaseModel
{
    public override void PropertyChangedOnInit()
    {
        OnPropertyChanged(nameof(HP));
        OnPropertyChanged(nameof(MP));
        OnPropertyChanged(nameof(EXP));
        OnPropertyChanged(nameof(MagnetRadius));
        OnPropertyChanged(nameof(DashCount)); 
        OnPropertyChanged(nameof(DashMaxCount)); 
        OnPropertyChanged(nameof(DashCoolTime));
        OnPropertyChanged(nameof(DashChargeTime));
    }

    private float _hp;
    public float HP
    {
        get { return _hp; }
        set
        {
            if (_hp == value)
            {
                return;
            }
            _hp = value;

            if(_hp < 0)
            {
                _hp = 0;
            }
            OnPropertyChanged(nameof(HP));
        }
    }

    private float _mp;
    public float MP
    {
        get { return _mp; }
        set
        {
            if (_mp == value)
            {
                return;
            }
            _mp = value;
            OnPropertyChanged(nameof(MP));
        }
    }

    private float _exp;
    public float EXP
    {
        get { return _exp; }
        set
        {
            if (_exp == value)
            {
                return;
            }
            _exp = value;
            OnPropertyChanged(nameof(EXP));
        }
    }

    private float _magnetRadius;
    public float MagnetRadius
    {
        get { return _magnetRadius; }
        set
        {
            if (_magnetRadius == value)
            {
                return;
            }
            _magnetRadius = value;
            OnPropertyChanged(nameof(MagnetRadius));
        }
    }

    private int _dashCount;
    public int DashCount
    {
        get { return _dashCount; }
        set
        {
            if (_dashCount == value)
            {
                return;
            }
            _dashCount = value;
            OnPropertyChanged(nameof(DashCount));
        }
    }

    private int _dashMaxCount;
    public int DashMaxCount
    {
        get { return _dashMaxCount; }
        set
        {
            if (_dashMaxCount == value)
            {
                return;
            }
            _dashMaxCount = value;
            OnPropertyChanged(nameof(DashMaxCount));
        }
    }

    private float _dashChargeTime;
    public float DashChargeTime
    {
        get { return _dashChargeTime; }
        set
        {
            if (_dashChargeTime == value)
            {
                return;
            }
            _dashChargeTime = value;
            OnPropertyChanged(nameof(DashChargeTime));
        }
    }

    private float _dashCoolTime;
    public float DashCoolTime
    {
        get { return _dashCoolTime; }
        set
        {
            if (_dashCoolTime == value)
            {
                return;
            }
            _dashCoolTime = value;
            OnPropertyChanged(nameof(DashCoolTime));
        }
    }
}
