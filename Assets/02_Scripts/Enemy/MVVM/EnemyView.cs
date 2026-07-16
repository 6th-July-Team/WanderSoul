using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using Unity.Behavior;
using UnityEngine;
using UnityEngine.AI;

public class EnemyView : BaseView<EnemyViewModel>, IDamageable, ISensorListener
{
    public Vector3 Position => this.transform.position;
    public bool IsAlive => _viewModel.EnemyState != BT_EnemyState.Dead;
    public EntityType EntityType => EntityType.Enemy;

    [SerializeField] private EnemyTargetSensor EnemySensor_Self;
    private EnemyTargetSelector _targetSelector;

    [SerializeField] private BehaviorGraphAgent BehaviorGraphAgent_Self;
    [SerializeField] private NavMeshAgent NavMeshAgent_Self;
    [SerializeField] private Animator Animator_Self;

    private static readonly int ATTACK_HASH = Animator.StringToHash("Attack");
    private static readonly int GET_DAMAGE_HASH = Animator.StringToHash("GetDamage");
    private static readonly int IS_DEAD_HASH = Animator.StringToHash("IsDead");

    private const float TARGET_EXCLUDE_DURATION = 15f;

    public GameObject CurrentTarget => _targetSelector.CurrentTarget;

    private GameObject _wagon;
    private GameObject _player;

    public event System.Action OnEnemyDied;

    // Melee 전용
    [SerializeField] private EnemyAttackHitbox AttackHitbox_Self;

    // Projectile 전용
    [SerializeField] private Transform ShootPoint_Self;
    private GameObject _prefab_projectile;

    #region Init
    public void Init(GameObject wagon, GameObject player)
    {
        _wagon = wagon;
        _player = player;


        CreateTargetSelector();
        SetEnemySensor();
        SetAvoidancePriority();
        SetBehaviorGraphAgent();

        if (_viewModel.EnemyAttackType == EnemyAttackType.Projectile)
        {
            SetProjectilePrefab().Forget();
        }
    }

    private void SetBehaviorGraphAgent()
    {
        BehaviorGraphAgent_Self.SetVariableValue("Wagon", _wagon);
    }

    private void SetAvoidancePriority()
    {
        switch (_viewModel.EnemyAttackType)
        {
            case EnemyAttackType.Melee:
                {
                    NavMeshAgent_Self.avoidancePriority = Random.Range(10, 40);
                }
                break;
            case EnemyAttackType.Projectile:
                {
                    NavMeshAgent_Self.avoidancePriority = Random.Range(50, 100);
                }
                break;
            default:
                {

                }
                break;
        }
    }

    private void CreateTargetSelector()
    {
        _targetSelector = new EnemyTargetSelector(_viewModel.TargetPolicy, _wagon, _player, _viewModel.LeashRange);
        RefreshTarget();
    }

    private void SetEnemySensor()
    {
        EnemySensor_Self.SetRange(_viewModel.DetectRange);
    }

    private async UniTaskVoid SetProjectilePrefab()
    {
        _prefab_projectile = await GameManager.Resource.LoadAsset<GameObject>(_viewModel.ProjectilePrefabAddress);

        if (_prefab_projectile == null)
        {
            Debug.LogError("EnemyView : Prefab_Projectile이 로드되지 않았습니다!!");
        }
    }
    #endregion

    #region OnPropertyChanged
    protected override void OnPropertyChanged(string propertyName)
    {
        switch (propertyName)
        {
            case nameof(_viewModel.MaxHp):
                {
                    // [TODO : 이기웅] 최대 체력이 바뀔 경우
                }
                break;
            case nameof(_viewModel.Hp):
                {

                }
                break;
            case nameof(_viewModel.Attack):
                {
                    // [TODO : 이기웅] 공격력이 바뀔 경우
                }
                break;
            case nameof(_viewModel.AttackRange):
                {
                    if (_viewModel.EnemyAttackType == EnemyAttackType.Melee)
                    {
                        AttackHitbox_Self.SetRange(_viewModel.AttackRange);
                    }
                }
                break;
            case nameof(_viewModel.AttackSpeed):
                {
                    // [TODO : 이기웅] 공격속도가 바뀔 경우
                }
                break;
            case nameof(_viewModel.MoveSpeed):
                {
                    BehaviorGraphAgent_Self.SetVariableValue("MoveSpeed", _viewModel.MoveSpeed);
                    BehaviorGraphAgent_Self.SetVariableValue("ApproachSpeed", _viewModel.MoveSpeed * 2);
                }
                break;
            case nameof(_viewModel.EnemyState):
                {
                    BehaviorGraphAgent_Self.SetVariableValue("BT_EnemyState", _viewModel.EnemyState); // BG의 상태를 변경된 EnemyState로 변하게 함

                    SetActiveSensor();
                }
                break;
        }
    }
    #endregion

    #region Sensor
    private void SetActiveSensor()
    {
        bool isSensorActive = (_viewModel.EnemyState != BT_EnemyState.Approach);
        EnemySensor_Self.gameObject.SetActive(isSensorActive);
    }

    public void OnSensorChanged()
    {
        RefreshTarget();
    }
    #endregion

    // BG에서 상태 변경을 받기 위함
    public void RequestStateChange(BT_EnemyState newEnemyState)
    {
        _viewModel.ChangeState(newEnemyState);
    }

    // 타겟을 변경하고 BG에 알리기 위함
    public void RefreshTarget()
    {
        if (_wagon == null || _viewModel == null || _targetSelector == null)
        {
            return;
        }

        GameObject target = _targetSelector.SelectTarget(EnemySensor_Self.Candidates);
        bool isTargetDetected = (target != null && target != _wagon);

        BehaviorGraphAgent_Self.SetVariableValue("Target", target);
        BehaviorGraphAgent_Self.SetVariableValue("IsTargetDetected", isTargetDetected);
    }

    public void PlayAttackAction()
    {
        Animator_Self.SetTrigger(ATTACK_HASH);
    }

    public void PlayDeadAction()
    {
        Animator_Self.SetBool(IS_DEAD_HASH, true);
    }

    #region ExcuteAttack
    public void ExecuteAttack()
    {
        if (_viewModel.EnemyState != BT_EnemyState.Attack)
        {
            return;
        }

        switch (_viewModel.EnemyAttackType)
        {
            case EnemyAttackType.Melee:
                {
                    ExecuteMeleeTypeAttack();
                }
                break;
            case EnemyAttackType.Projectile:
                {
                    ExecuteProjectileTypeAttack();
                }
                break;
        }
    }

    private void ExecuteMeleeTypeAttack()
    {
        if (AttackHitbox_Self == null)
        {
            return;
        }

        IReadOnlyList<IDamageable> targetList = AttackHitbox_Self.TargetList;

        _viewModel.MeleeTypeAttack(targetList, transform.position);
    }

    private void ExecuteProjectileTypeAttack()
    {
        if (_prefab_projectile == null || CurrentTarget == null)
        {
            return;
        }

        EnemyProjectileObject projectileObject = CreateAProjectile();
        projectileObject.OnDamageableTargetHited += _viewModel.ProjectileTypeAttack;

        projectileObject.Launch(CurrentTarget, _viewModel.ProjectileSpeed, _viewModel.ProjectileLifeTime);
    }

    private EnemyProjectileObject CreateAProjectile()
    {
        GameObject projectileObjectGameObject = Instantiate(_prefab_projectile, ShootPoint_Self.position, Quaternion.identity);
        EnemyProjectileObject projectileObject = projectileObjectGameObject.GetComponent<EnemyProjectileObject>();

        return projectileObject;
    }

    #endregion

    public void ExecuteDead()
    {
        if (_viewModel.EnemyState != BT_EnemyState.Dead)
        {
            return;
        }

        OnEnemyDied?.Invoke();
        GameManager.Pool.DespawnToPool(this.gameObject);
    }

    public void TakeDamage(DamageInfo damageInfo)
    {
        if (_viewModel.EnemyState == BT_EnemyState.Dead)
        {
            return;
        }

        int damage = Mathf.RoundToInt(damageInfo.DamageAmount);

        bool isDamaged = _viewModel.TakeDamage(damage);

        if (isDamaged == false)
        {
            return;
        }

        if (_viewModel.EnemyState == BT_EnemyState.Dead)
        {
            Animator_Self.ResetTrigger(GET_DAMAGE_HASH);
            return;
        }

        Animator_Self.SetTrigger(GET_DAMAGE_HASH);
    }

    public void TryEnterAttackState(GameObject target)
    {
        if (target == null)
        {
            return;
        }

        float distanceToTarget = GetDistanceToTarget(target);

        _viewModel.TryEnterAttackState(distanceToTarget); // [TODO?] 지금은 Bool값을 안 사용하고 버리고 있지만 나중에 언젠간 쓸 예정
    }

    public void TryExitAttackState(GameObject target)
    {
        if (target == null)
        {
            return;
        }

        float distanceToTarget = GetDistanceToTarget(target);

        _viewModel.TryExitAttackState(distanceToTarget); // [TODO?] 지금은 Bool값을 안 사용하고 버리고 있지만 나중에 언젠간 쓸 예정#2
    }

    private float GetDistanceToTarget(GameObject target)
    {
        Vector3 selfPosition = transform.position;

        if (target.TryGetComponent(out Collider targetCollider))
        {
            Vector3 closestPoint = targetCollider.ClosestPoint(selfPosition);

            float distanceToTargetClosestPoint = Vector3.Distance(closestPoint, selfPosition);
            return distanceToTargetClosestPoint;
        }

        float distanceToTarget = Vector3.Distance(target.transform.position, selfPosition);

        return distanceToTarget;
    }

    // Navigate 노드가 "막힘"을 보고할 때 호출 — 후순위 타겟으로 전환 성공 시 true
    public bool TryFallbackTarget()
    {
        if (_targetSelector == null)
        {
            return false;
        }

        GameObject previousTarget = _targetSelector.CurrentTarget;
        GameObject newTarget = _targetSelector.ExcludeCurrentAndReselect(EnemySensor_Self.Candidates, TARGET_EXCLUDE_DURATION);

        if (newTarget == null || newTarget == previousTarget)
        {
            return false;
        }

        BehaviorGraphAgent_Self.SetVariableValue("Target", newTarget);
        BehaviorGraphAgent_Self.SetVariableValue("IsTargetDetected", newTarget != _wagon);
        return true;
    }

    #if UNITY_EDITOR
    [ContextMenu("TakeDamage")]
    private void TakeDamage()
    {
        Vector3 hitDirection = new Vector3(0, 0, this.transform.position.z + 1).normalized;

        DamageInfo testDamageInfo = new DamageInfo(10, hitDirection);

        TakeDamage(testDamageInfo);
    }
    #endif
}