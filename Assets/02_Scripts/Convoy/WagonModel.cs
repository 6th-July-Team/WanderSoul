using System.ComponentModel;

public class WagonModel : BaseModel
{
    public override void PropertyChangedOnInit()
    {
        OnPropertyChanged(nameof(Durability));
        OnPropertyChanged(nameof(MoveSpeed));
        OnPropertyChanged(nameof(EnemyCount));
        OnPropertyChanged(nameof(WarningTime));
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
}
