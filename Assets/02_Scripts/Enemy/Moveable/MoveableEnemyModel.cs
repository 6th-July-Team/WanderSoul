
public class MoveableEnemyModel : BaseModel
{
    // 프로퍼티가 추가될 때마다 계속 추가하기!! 깜빡하지 말기!!
    public override void PropertyChangedOnInit()
    {
        OnPropertyChanged(nameof(MaxHp));
        OnPropertyChanged(nameof(Hp));

        OnPropertyChanged(nameof(DetectRange));
        OnPropertyChanged(nameof(LeashRange));

        OnPropertyChanged(nameof(Attack));
        OnPropertyChanged(nameof(AttackRange));
        OnPropertyChanged(nameof(AttackSpeed));

        OnPropertyChanged(nameof(PreferredDistance));

        OnPropertyChanged(nameof(MoveSpeed));
        OnPropertyChanged(nameof(EnemyState));
    }

    public MoveableEnemyModel(EnemyData enemyData)
    {
        _policy = enemyData.Policy;
        _attackType = enemyData.AttackType;

        _maxHp = enemyData.MaxHp;
        _hp = enemyData.MaxHp;

        _detectRange = enemyData.DetectRange;
        _leashRange = enemyData.LeashRange;

        _attack = enemyData.Attack;
        _attackRange = enemyData.AttackRange;
        _attackSpeed = enemyData.AttackSpeed;

        _preferredDistance = enemyData.PreferredDistance;

        _moveSpeed = enemyData.MoveSpeed;
        _enemyState = BT_MoveableEnemyState.Approach;
    }

    private TargetPolicy _policy;
    public TargetPolicy Policy
    {
        get { return _policy; }
    }

    private EnemyAttackType _attackType;
    public EnemyAttackType AttackType
    {
        get { return _attackType; }
    }

    private int _maxHp;
    public int MaxHp
    {
        get => _maxHp;
        set
        {
            if (_maxHp != value)
            {
                _maxHp = value;
                OnPropertyChanged(nameof(MaxHp));
            }
        }
    }

    private int _hp;
    public int Hp
    {
        get => _hp;
        set
        {
            if (_hp != value)
            {
                _hp = value;
                OnPropertyChanged(nameof(Hp));
            }
        }
    }

    private float _detectRange;
    public float DetectRange
    {
        get => _detectRange;
        set
        {
            if (_detectRange != value)
            {
                _detectRange = value;
                OnPropertyChanged(nameof(DetectRange));
            }
        }
    }

    private float _leashRange;
    public float LeashRange
    {
        get => _leashRange;
        set
        {
            if (_leashRange != value)
            {
                _leashRange = value;
                OnPropertyChanged(nameof(LeashRange));
            }
        }
    }

    private float _preferredDistance;
    public float PreferredDistance
    {
        get => _preferredDistance;
        set
        {
            if (_preferredDistance != value)
            {
                _preferredDistance = value;
                OnPropertyChanged(nameof(PreferredDistance));
            }
        }
    }

    private int _attack;
    public int Attack
    {
        get => _attack;
        set
        {
            if (_attack != value)
            {
                _attack = value;
                OnPropertyChanged(nameof(Attack));
            }
        }
    }

    private float _attackRange;
    public float AttackRange
    {
        get => _attackRange;
        set
        {
            if (_attackRange != value)
            {
                _attackRange = value;
                OnPropertyChanged(nameof(AttackRange));
            }
        }
    }

    private float _attackSpeed;
    public float AttackSpeed
    {
        get => _attackSpeed;
        set
        {
            if (_attackSpeed != value)
            {
                _attackSpeed = value;
                OnPropertyChanged(nameof(AttackSpeed));
            }
        }
    }

    private float _moveSpeed;
    public float MoveSpeed
    {
        get => _moveSpeed;
        set
        {
            if (_moveSpeed != value)
            {
                _moveSpeed = value;
                OnPropertyChanged(nameof(MoveSpeed));
            }
        }
    }

    private BT_MoveableEnemyState _enemyState;
    public BT_MoveableEnemyState EnemyState
    {
        get => _enemyState;
        set
        {
            if (_enemyState != value)
            {
                _enemyState = value;
                OnPropertyChanged(nameof(EnemyState));
            }
        }
    }
}
