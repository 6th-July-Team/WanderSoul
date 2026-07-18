using UnityEngine;

public class EnemyViewModel : BaseViewModel<EnemyModel>
{
    public TargetPolicy TargetPolicy => _model.Policy;
    public EnemyAttackType EnemyAttackType => _model.AttackType;
    public DamageType DamageType => _model.DamageType;

    public string Name => _model.Name;
    public string Description => _model.Description;

    public int MaxHp => _model.MaxHp;
    public int Hp => _model.Hp;
    
    public float DetectRange => _model.DetectRange;
    public float LeashRange => _model.LeashRange;
    
    public int Attack => _model.Attack;
    public float AttackRange => _model.AttackRange;
    public float AttackSpeed => _model.AttackSpeed;

    public float SoulDropChance => _model.SoulDropChance;
    public float SoulDropAmount => _model.SoulDropAmount;

    public float ExpDropChance => _model.ExpDropChance;
    public float ExpDropAmount => _model.ExpDropAmount;

    public bool CanMove => _model.CanMove;
    public float MoveSpeed => _model.MoveSpeed;
    public BT_EnemyState EnemyState => _model.EnemyState;

    //투사체 발사(원거리) 전용
    public float ProjectileSpeed => _model.ProjectileSpeed;
    public float ProjectileLifeTime => _model.ProjectileLifeTime;
    public string ProjectilePrefabAddress => _model.ProjectilePrefabAddress;

    //고정 포대형(예고 후 장판 공격) 전용
    public float AreaRadius => _model.AreaRadius;
    public float AreaDelayTime => _model.AreaDelayTime;
    public string AreaPrefabAddress => _model.AreaPrefabAddress;

    public EnemyViewModel(EnemyModel model) : base(model) { }

    public bool TakeDamage(int damage)
    {
        if (_model.EnemyState == BT_EnemyState.Dead)
        {
            return false;
        }

        if(damage <= 0)
        {
            return false;
        }

        int damagedHp = _model.Hp - damage;

        int newHp = Mathf.Max(0, damagedHp);
        _model.Hp = newHp;

        if(newHp == 0)
        {
            _model.EnemyState = BT_EnemyState.Dead;
        }

        return true;
    }

    public bool TryEnterAttackState(float distanceToTarget)
    {
        bool canEnterAttack = (_model.EnemyState == BT_EnemyState.Chase || _model.EnemyState == BT_EnemyState.Idle);

        if(canEnterAttack == false)
        {
            return false;
        }

        float enterAttackRange = 0.8f;

        float attackRange = _model.AttackRange * enterAttackRange;

        if(distanceToTarget > attackRange)
        {
            return false;
        }

        _model.EnemyState = BT_EnemyState.Attack;
        return true;
    }

    public bool TryExitAttackState(float distanceToTarget)
    {
        if(_model.EnemyState != BT_EnemyState.Attack)
        {
            return false;
        }

        float exitAttackRange = _model.AttackRange;

        if(distanceToTarget <= exitAttackRange)
        {
            return false;
        }

        switch (_model.CanMove)
        {
            case true:
                {
                    _model.EnemyState = BT_EnemyState.Chase;
                }
                break;
            case false:
                {
                    _model.EnemyState = BT_EnemyState.Idle;
                }
                break;
        }

        return true;
    }

    public void ChangeState(BT_EnemyState newEnemyState)
    {
        if(_model.EnemyState == BT_EnemyState.Dead)
        {
            return;
        }

        bool isCannotMoveEnemyState = (newEnemyState == BT_EnemyState.Approach || newEnemyState == BT_EnemyState.Chase);

        if (_model.CanMove == false && isCannotMoveEnemyState)
        {
            return;
        }

        _model.EnemyState = newEnemyState;
    }
}
