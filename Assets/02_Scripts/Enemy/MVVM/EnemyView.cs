using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Threading;
using Unity.Behavior;
using UnityEngine;
using UnityEngine.AI;

public class EnemyView : BaseView<EnemyViewModel>, IEnemy, ISensorListener
{
    public Vector3 Position => this.transform.position;
    public Transform Transform => this.transform;
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

    private static readonly (DropObjectDigit digit, int value)[] DIGIT_TABLE =
    {
        (DropObjectDigit.Hundred, 100),
        (DropObjectDigit.Ten, 10),
        (DropObjectDigit.One, 1),
    };

    public ITargetable CurrentTarget => _targetSelector.CurrentTarget;

    private GameObject _tauntEffect;
    private CancellationTokenSource _tauntCts;

    private GameObject _wagon;
    private GameObject _player;
    private ITargetable _wagonTargetable;
    private ITargetable _playerTargetable;

    public event System.Action OnEnemyDied;

    [SerializeField] private float DeadDelay;

    [Header("Melee 전용")]
    [SerializeField] private EnemyAttackHitbox AttackHitbox_Self;

    [Header("Projectile 전용")]
    [SerializeField] private Transform ShootPoint_Self;
    private GameObject _prefab_projectile;

    // AreaDelayed 전용
    private GameObject _prefab_areaDelay;
    private LayerMask _groundLayerMask;

    #region Init
    public void Init(GameObject wagon, GameObject player)
    {
        _wagon = wagon;
        _player = player;

        SetWagonAndPlayerTargetable();

        SetBehaviorGraphAgent();
        SetAvoidancePriority();
        CreateTargetSelector();
        SetEnemySensor();

        if (_viewModel.EnemyAttackType == EnemyAttackType.Projectile)
        {
            SetProjectileObjectPrefab().Forget();
        }

        if (_viewModel.EnemyAttackType == EnemyAttackType.AreaDelayed)
        {
            SetAreaDelayObjectPrefab().Forget();
            SetGroundLayerMask();
        }
    }

    private void SetWagonAndPlayerTargetable()
    {
        _wagon.TryGetComponent(out _wagonTargetable);
        _player.TryGetComponent(out _playerTargetable);

        if (_wagonTargetable == null)
        {
            Debug.LogError("EnemyView : Wagon에 ITargetable 컴포넌트가 없습니다!!");
        }

        if (_playerTargetable == null)
        {
            Debug.LogError("EnemyView : Player에 ITargetable 컴포넌트가 없습니다!!");
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
            case EnemyAttackType.AreaDelayed:
                {
                    NavMeshAgent_Self.avoidancePriority = 0;
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
        _targetSelector = new EnemyTargetSelector(_viewModel.TargetPolicy, _wagonTargetable, _playerTargetable, _viewModel.LeashRange);
        RefreshTarget();
    }

    private void SetEnemySensor()
    {
        EnemySensor_Self.SetRange(_viewModel.DetectRange);
    }

    private async UniTaskVoid SetProjectileObjectPrefab()
    {
        _prefab_projectile = await GameManager.Resource.LoadAsset<GameObject>(_viewModel.ProjectilePrefabAddress);

        if (_prefab_projectile == null)
        {
            Debug.LogError("EnemyView : Prefab_Projectile이 로드되지 않았습니다!!");
        }
    }

    private async UniTaskVoid SetAreaDelayObjectPrefab()
    {
        _prefab_areaDelay = await GameManager.Resource.LoadAsset<GameObject>(_viewModel.AreaPrefabAddress);

        if (_prefab_areaDelay == null)
        {
            Debug.LogError("EnemyView : Prefab_AreaDelay이 로드되지 않았습니다!!");
        }
    }

    private void SetGroundLayerMask()
    {
        _groundLayerMask = LayerMask.GetMask("Ground");
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
                    BehaviorGraphAgent_Self.SetVariableValue("AttackDelay", _viewModel.AttackSpeed);
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

    private void OnDisable()
    {
        CancelTaunt();
        SetTauntEffect(false);
    }

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

        ITargetable target = _targetSelector.SelectTarget(EnemySensor_Self.Candidates);
        GameObject targetObject = GetTargetGameObject(target);

        bool isTargetDetected = (targetObject != null && targetObject != _wagon);

        BehaviorGraphAgent_Self.SetVariableValue("Target", targetObject);
        BehaviorGraphAgent_Self.SetVariableValue("IsTargetDetected", isTargetDetected);
    }

    private GameObject GetTargetGameObject(ITargetable target)
    {
        Component targetComponent = target as Component;

        if (targetComponent == null)
        {
            return null;
        }

        return targetComponent.gameObject;
    }

    #region Animation

    public void PlayAttackAction()
    {
        Animator_Self.ResetTrigger(ATTACK_HASH);
        Animator_Self.SetTrigger(ATTACK_HASH);
    }

    public void PlayDeadAction()
    {
        Animator_Self.SetBool(IS_DEAD_HASH, true);
    }

    #endregion

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
            case EnemyAttackType.AreaDelayed:
                {
                    ExecuteAreaDelayedTypeAttack();
                }
                break;
        }
    }

    #region ExecuteAttack
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
        GameObject targetObject = GetTargetGameObject(CurrentTarget);

        if (_prefab_projectile == null || targetObject == null)
        {
            return;
        }

        EnemyProjectileObject projectileScript = CreateAProjectile();
        projectileScript.OnDamageableTargetHited += _viewModel.ProjectileTypeAttack;

        projectileScript.Launch(targetObject, _viewModel.ProjectileSpeed, _viewModel.ProjectileLifeTime);
    }

    private EnemyProjectileObject CreateAProjectile()
    {
        GameObject projectileObject = Instantiate(_prefab_projectile, ShootPoint_Self.position, Quaternion.identity);
        EnemyProjectileObject projectileScript = projectileObject.GetComponent<EnemyProjectileObject>();

        return projectileScript;
    }

    private void ExecuteAreaDelayedTypeAttack()
    {
        if (_prefab_areaDelay == null || CurrentTarget == null)
        {
            return;
        }

        Vector3 targetGroundPosition = GetGroundPosition(CurrentTarget.Position);

        EnemyAreaDelayObject areaDelayScript = CreateAreaDelay(targetGroundPosition);
        areaDelayScript.OnDamageableTargetHited += _viewModel.AreaDelayedTypeAttack;

        areaDelayScript.Deploy(_viewModel.AreaRadius, _viewModel.AreaDelayTime);
    }

    private Vector3 GetGroundPosition(Vector3 targetPosition)
    {
        Ray ray = new Ray(targetPosition + new Vector3(0, 3, 0), Vector3.down);

        if (Physics.Raycast(ray, out RaycastHit hit, 50f, _groundLayerMask) == false)
        {
            Logger.LogWarning("Ray를 맞추는 데 실패했습니다!! 기본 targetPosition을 return합니다!");
            return targetPosition;
        }

        return hit.point;
    }

    private EnemyAreaDelayObject CreateAreaDelay(Vector3 targetPosition)
    {
        GameObject areaDelayObject = Instantiate(_prefab_areaDelay, targetPosition, Quaternion.identity);
        EnemyAreaDelayObject areaDelayScript = areaDelayObject.GetComponent<EnemyAreaDelayObject>();

        return areaDelayScript;
    }

    #endregion

    public void ExecuteDead()
    {
        if (_viewModel.EnemyState != BT_EnemyState.Dead)
        {
            return;
        }

        CancelTaunt();
        SetTauntEffect(false);

        OnEnemyDied?.Invoke();

        // TODO : 사망 이펙트 추가
        SpawnDropObjects(DropObjectType.Soul);
        SpawnDropObjects(DropObjectType.Exp);
        DespawnAfterDelay().Forget();
    }

    #region ExecuteDead
    private void SpawnDropObjects(DropObjectType type)
    {
        if (_viewModel.TryRollDrop(type) == false)
        {
            return;
        }

        int amount = _viewModel.GetDropAmount(type);

        foreach ((DropObjectDigit digit, int value) in DIGIT_TABLE)
        {
            int count = amount / value;
            amount %= value;

            for (int i = 0; i < count; i++)
            {
                SpawnOneDropObject(type, digit);
            }
        }
    }

    private void SpawnOneDropObject(DropObjectType type, DropObjectDigit digit)
    {
        string poolId = $"EnemyDropObject/{type}/{digit}";

        EnemyDropObject dropObject = GameManager.Pool.SpawnFromPool<EnemyDropObject>(poolId, GetDropScatterPosition());
        dropObject.Init(type, digit);
    }

    private Vector3 GetDropScatterPosition()
    {
        Vector2 scatterOffset = Random.insideUnitCircle * 0.5f;

        return transform.position + new Vector3(scatterOffset.x, 0.1f, scatterOffset.y);
    }

    private async UniTaskVoid DespawnAfterDelay()
    {
        await UniTask.Delay(System.TimeSpan.FromSeconds(DeadDelay));
        GameManager.Pool.DespawnToPool(this.gameObject);
    }

    #endregion

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

    public bool TryExitAttackState(GameObject target)
    {
        if (target == null)
        {
            return true;
        }

        float distanceToTarget = GetDistanceToTarget(target);

        bool isExitAttackState = _viewModel.TryExitAttackState(distanceToTarget);
        return isExitAttackState;
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

        ITargetable previousTarget = _targetSelector.CurrentTarget;
        ITargetable newTarget = _targetSelector.ExcludeCurrentAndReselect(EnemySensor_Self.Candidates, TARGET_EXCLUDE_DURATION);

        if (newTarget == null || newTarget == previousTarget)
        {
            return false;
        }

        GameObject newTargetObject = GetTargetGameObject(newTarget);

        BehaviorGraphAgent_Self.SetVariableValue("Target", newTargetObject);
        BehaviorGraphAgent_Self.SetVariableValue("IsTargetDetected", newTargetObject != _wagon);
        return true;
    }

    #region Taunt
    public void ApplyTaunt(IPet taunter, float duration)
    {
        if (_viewModel.EnemyState == BT_EnemyState.Dead)
        {
            return;
        }

        _targetSelector.SetForcedTarget(taunter, duration);
        RefreshTarget();

        SetTauntEffect(true);

        CancelTaunt();
        _tauntCts = CancellationTokenSource.CreateLinkedTokenSource(this.GetCancellationTokenOnDestroy());

        ReleaseTauntAfterDelay(duration, _tauntCts.Token).Forget();
    }

    private async UniTaskVoid ReleaseTauntAfterDelay(float duration, CancellationToken token)
    {
        await UniTask.Delay(System.TimeSpan.FromSeconds(duration), cancellationToken: token);
        _targetSelector.ClearForcedTarget();

        SetTauntEffect(false);
        RefreshTarget();
    }

    private void CancelTaunt()
    {
        _tauntCts?.Cancel();
        _tauntCts?.Dispose();
        _tauntCts = null;
    }

    private void SetTauntEffect(bool isActive)
    {
        if (_tauntEffect == null)
        {
            if (isActive == false)
            {
                return;
            }

            GameObject prefab = Utils.ResourcesLoad<GameObject>("TauntEffect");

            if (prefab == null)
            {
                return;
            }

            _tauntEffect = Instantiate(prefab, this.transform);
            _tauntEffect.transform.localPosition = GetTauntEffectOffset();
        }

        _tauntEffect.SetActive(isActive);
    }

    private Vector3 GetTauntEffectOffset()
    {
        if(this.TryGetComponent(out Collider collider_self))
        {
            Vector3 tauntEffectOffest = new(0, collider_self.bounds.size.y, 0);
            return tauntEffectOffest;
        }

        return new Vector3(0, 2f, 0);
    }
    #endregion

    public bool ShouldRetreatFromTarget(GameObject target)
    {
        if (target == null)
        {
            return false;
        }

        float distanceToTarget = GetDistanceToTarget(target);

        return _viewModel.ShouldRetreat(distanceToTarget);
    }

    public bool TryGetRetreatPosition(GameObject target, out Vector3 retreatPosition)
    {
        retreatPosition = this.transform.position;

        if (target == null)
        {
            return false;
        }

        Vector3 awayDirection = (this.transform.position - target.transform.position);
        awayDirection.y = 0f;
        awayDirection.Normalize();

        Vector3 desiredPosition = target.transform.position + awayDirection * _viewModel.PreferredDistance;

        if (NavMesh.SamplePosition(desiredPosition, out NavMeshHit hit, 2f, NavMesh.AllAreas) == false)
        {
            return false;
        }

        retreatPosition = hit.position;
        return true;
    }


#if UNITY_EDITOR
    [ContextMenu("TakeDamage")]
    private void TakeDamage()
    {
        Vector3 hitDirection = new Vector3(0, 0, this.transform.position.z + 1).normalized;

        DamageInfo testDamageInfo = new DamageInfo(_viewModel.MaxHp * 0.5f, hitDirection, DamageType.None);

        TakeDamage(testDamageInfo);
    }
#endif
}