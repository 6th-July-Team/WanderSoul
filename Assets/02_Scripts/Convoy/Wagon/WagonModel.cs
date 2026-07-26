

public class WagonModel : BaseModel
{
    public override void PropertyChangedOnInit()
    {
        OnPropertyChanged(nameof(Durability));
        OnPropertyChanged(nameof(MoveSpeed));
        OnPropertyChanged(nameof(EnemyCount));
        OnPropertyChanged(nameof(WarningTime));
        OnPropertyChanged(nameof(Name));
        OnPropertyChanged(nameof(Capacity));
        OnPropertyChanged(nameof(Progress)); 
        OnPropertyChanged(nameof(ConvoyTimer));
        OnPropertyChanged(nameof(IsWarningActive));
    }

    private float _durability;
    public float Durability
    {
        get { return _durability; }
        set
        {
            if (_durability == value)
            {
                return;
            }
            _durability = value;
            OnPropertyChanged(nameof(Durability));
        }
    }

    private float _moveSpeed;
    public float MoveSpeed
    {
        get { return _moveSpeed; }
        set
        {
            if (_moveSpeed == value)
            {
                return;
            }
            _moveSpeed = value;
            OnPropertyChanged(nameof(MoveSpeed));
        }
    }

    private int _enemyCount;
    public int EnemyCount
    {
        get { return _enemyCount; }
        set
        {
            if (_enemyCount == value)
            {
                return;
            }
            _enemyCount = value;
            OnPropertyChanged(nameof(EnemyCount));
        }
    }

    private float _warningTime;
    public float WarningTime
    {
        get { return _warningTime; }
        set
        {
            if (_warningTime == value)
            {
                return;
            }
            _warningTime = value;
            OnPropertyChanged(nameof(WarningTime));
        }
    }

    private string _name;
    public string Name
    {
        get { return _name; }
        set
        {
            if (_name == value)
            {
                return;
            }
            _name = value;
            OnPropertyChanged(nameof(Name));
        }
    }

    private int _capacity;
    public int Capacity
    {
        get { return _capacity; }
        set
        {
            if (_capacity == value)
            {
                return;
            }
            _capacity = value;
            OnPropertyChanged(nameof(Capacity));
        }
    }

    private float _progress;
    public float Progress
    {
        get { return _progress; }
        set
        {
            if (_progress == value)
            {
                return;
            }
            _progress = value;
            OnPropertyChanged(nameof(Progress));
        }
    }

    private float _convoyTimer;
    public float ConvoyTimer
    {
        get { return _convoyTimer; }
        set
        {
            if (_convoyTimer == value)
            {
                return;
            }
            _convoyTimer = value;
            OnPropertyChanged(nameof(ConvoyTimer));
        }
    }

    private bool _isWarningActive;
    public bool IsWarningActive
    {
        get { return _isWarningActive; }
        set
        {
            if (_isWarningActive == value)
            {
                return;
            }
            _isWarningActive = value;
            OnPropertyChanged(nameof(IsWarningActive));
        }
    }
}