
public class EnemyModel : BaseModel
{
    // 프로퍼티가 추가될 때마다 계속 추가하기!! 깜빡하지 말기!!
    public override void PropertyChangedOnInit()
    {
        OnPropertyChanged(nameof(MaxHp));
        OnPropertyChanged(nameof(Hp));

        OnPropertyChanged(nameof(Attack));
        OnPropertyChanged(nameof(AttackRange));
        OnPropertyChanged(nameof(AttackSpeed));

        OnPropertyChanged(nameof(MoveSpeed));
        OnPropertyChanged(nameof(EnemyState));
    }

    public EnemyModel(EnemyData enemyData)
    {
        _policy = enemyData.Policy;
        _attackType = enemyData.AttackType;
        _damageType = enemyData.Type;
        _prefabAddress = enemyData.PrefabAddress;

        _name = enemyData.Name;
        _description = enemyData.Description;

        _maxHp = enemyData.MaxHp;
        _hp = enemyData.MaxHp;

        _detectRange = enemyData.DetectRange;
        _leashRange = enemyData.LeashRange;

        _attack = enemyData.Attack;
        _attackRange = enemyData.AttackRange;
        _attackSpeed = enemyData.AttackSpeed;

        _soulDropChance = enemyData.SoulDropChance;
        _soulDropAmount = enemyData.SoulDropAmount;
        
        _expDropChance = enemyData.ExpDropChance;
        _expDropAmount = enemyData.ExpDropAmount;

        _canMove = enemyData.CanMove;
        _moveSpeed = enemyData.MoveSpeed;

        switch(_canMove)
        {
            case true:
                {
                    _enemyState = BT_EnemyState.Approach;
                }
                break;
                case false:
                {
                    _enemyState = BT_EnemyState.Idle;
                }
                break;
        }

        if (_attackType == EnemyAttackType.Projectile)
        {
            _projectileSpeed = enemyData.ProjectileSpeed;
            _projectileLifeTime = enemyData.ProjectileLifeTime;
            _projectilePrefabAddress = enemyData.ProjectilePrefabAddress;
        }

        if(_attackType == EnemyAttackType.AreaDelayed)
        {
            _areaRadius = enemyData.AreaRadius;
            _areaDelayTime = enemyData.AreaDelayTime;
            _areaPrefabAddress = enemyData.AreaPrefabAddress;
        }
    }

    private TargetPolicy _policy;
    public TargetPolicy Policy
    {
        get => _policy;
    }

    private bool _canMove;
    public bool CanMove
    {
        get => _canMove;
    }

    private EnemyAttackType _attackType;
    public EnemyAttackType AttackType
    {
        get => _attackType;    
    }

    private DamageType _damageType;
    public DamageType DamageType
    {
        get => _damageType;
    }

    private string _prefabAddress;
    public string PrefabAddress
    {
        get => _prefabAddress;
    }

    private string _name;
    public string Name
    {
        get => _name;
    }

    private string _description;
    public string Description
    {
        get => _description;
    }

    private float _detectRange;
    public float DetectRange
    {
        get => _detectRange;
    }

    private float _leashRange;
    public float LeashRange
    {
        get => _leashRange;
    }

    private float _soulDropChance;
    public float SoulDropChance
    {
        get => _soulDropChance;
    }

    private float _soulDropAmount;
    public float SoulDropAmount
    {
        get => _soulDropAmount;
    }

    private float _expDropChance;
    public float ExpDropChance
    {
        get => _expDropChance;
    }

    private float _expDropAmount;
    public float ExpDropAmount
    {
        get => _expDropAmount;
    }

    private float _projectileSpeed;
    public float ProjectileSpeed
    {
        get => _projectileSpeed;
    }

    private float _projectileLifeTime;
    public float ProjectileLifeTime
    {
        get => _projectileLifeTime;
    }

    private string _projectilePrefabAddress;
    public string ProjectilePrefabAddress
    {
        get => _projectilePrefabAddress;
    }

    private float _areaRadius;
    public float AreaRadius
    {
        get => _areaRadius;
    }

    private float _areaDelayTime;
    public float AreaDelayTime
    {
        get => _areaDelayTime;
    }

    private string _areaPrefabAddress;
    public string AreaPrefabAddress
    {
        get => _areaPrefabAddress;
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

    private BT_EnemyState _enemyState;
    public BT_EnemyState EnemyState
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


