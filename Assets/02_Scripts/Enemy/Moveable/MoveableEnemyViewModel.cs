using UnityEngine;

public class MoveableEnemyViewModel : BaseViewModel<MoveableEnemyModel>
{
    public TargetPolicy TargetPolicy => _model.Policy;
    public EnemyAttackType EnemyAttackType => _model.AttackType;
    public int MaxHp => _model.MaxHp;
    public int Hp => _model.Hp;
    public float DetectRange => _model.DetectRange;
    public float LeashRange => _model.LeashRange;
    public int Attack => _model.Attack;
    public float AttackRange => _model.AttackRange;
    public float AttackSpeed => _model.AttackSpeed;
    public float PreferredDistance => _model.PreferredDistance;
    public float MoveSpeed => _model.MoveSpeed;
    public BT_MoveableEnemyState EnemyState => _model.EnemyState;

    public MoveableEnemyViewModel(MoveableEnemyModel model) : base(model) { }

    public bool TakeDamage(int damage)
    {
        if (_model.EnemyState == BT_MoveableEnemyState.Dead)
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
            _model.EnemyState = BT_MoveableEnemyState.Dead;
        }

        return true;
    }

    public bool TryEnterAttackState(float distanceToTarget)
    {
        if(_model.EnemyState != BT_MoveableEnemyState.Chase)
        {
            return false;
        }

        float enterAttackRange = 0.8f;

        float attackRange = _model.AttackRange * enterAttackRange;

        if(distanceToTarget > attackRange)
        {
            return false;
        }

        _model.EnemyState = BT_MoveableEnemyState.Attack;
        return true;
    }

    public bool TryExitAttackState(float distanceToTarget)
    {
        if(_model.EnemyState != BT_MoveableEnemyState.Attack)
        {
            return false;
        }

        float exitAttackRange = _model.AttackRange;

        if(distanceToTarget <= exitAttackRange)
        {
            return false;
        }

        _model.EnemyState = BT_MoveableEnemyState.Chase;
        return true;
    }

    public void ChangeState(BT_MoveableEnemyState newEnemyState)
    {
        if(_model.EnemyState == BT_MoveableEnemyState.Dead)
        {
            return;
        }

        _model.EnemyState = newEnemyState;
    }
}
