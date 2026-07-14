using System.Collections.Generic;
using Unity.Behavior;
using UnityEngine;
using UnityEngine.AI;

public class MoveableEnemyView : BaseView<MoveableEnemyViewModel>, IDamageable, ISensorListener, IEnemyView
{
    [SerializeField] private EnemyTargetSensor EnemySensor_Self;
    [SerializeField] private EnemyAttackHitbox AttackHitbox_Self;
    private EnemyTargetSelector _targetSelector;

    [SerializeField] private BehaviorGraphAgent BehaviorGraphAgent_Self;
    [SerializeField] private NavMeshAgent NavMeshAgent_Self;
    [SerializeField] private Animator Animator_Self;

    private GameObject _wagon;
    private GameObject _player;

    public event System.Action OnEnemyDied;

    private static readonly int ATTACK_HASH = Animator.StringToHash("Attack");
    private static readonly int GET_DAMAGE_HASH = Animator.StringToHash("GetDamage");
    private static readonly int IS_DEAD_HASH = Animator.StringToHash("IsDead");

    private const float TARGET_EXCLUDE_DURATION = 15f;

    // [TODO] 나중에 wagon과 player를 스크립트로 받아오게 수정할 수 있음
    public void Init(GameObject wagon, GameObject player)
    {
        _wagon = wagon;
        _player = player;

        BehaviorGraphAgent_Self.SetVariableValue("Wagon", _wagon);

        NavMeshAgent_Self.avoidancePriority = Random.Range(30, 70); // 전원이 같은 우선순위(50)면 회피가 대칭 교착으로 떨림 — 개체별로 흩어서 양보 순서를 만듦

        CreateTargetSelector();
    }

    private void CreateTargetSelector()
    {
        _targetSelector = new EnemyTargetSelector(_viewModel.TargetPolicy, _wagon, _player, _viewModel.LeashRange);
        RefreshTarget();
    }

    // [TODO] 임시
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
                    // [TODO : 이기웅] 체력바가 있다면 체력바 갱신 로직 등등
                }
                break;
            case nameof(_viewModel.DetectRange):
                {
                    EnemySensor_Self.SetRange(_viewModel.DetectRange); // 센서 내부 Collider의 radius를 변경된 DetectRange 값으로 변하게 함
                }
                break;
            case nameof(_viewModel.LeashRange):
                {
                    // [TODO : 이기웅] 마차 탐색 범위가 바뀔 경우
                }
                break;
            case nameof(_viewModel.Attack):
                {
                    // [TODO : 이기웅] 공격력이 바뀔 경우
                }
                break;
            case nameof(_viewModel.AttackRange):
                {
                    AttackHitbox_Self.SetRange(_viewModel.AttackRange);
                }
                break;
            case nameof(_viewModel.AttackSpeed):
                {
                    // [TODO : 이기웅] 공격속도가 바뀔 경우
                }
                break;
            case nameof(_viewModel.PreferredDistance):
                {
                    // [TODO : 이기웅] 후퇴 범위가 바뀔 경우 
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
                    BehaviorGraphAgent_Self.SetVariableValue("BT_MoveableEnemyState", _viewModel.EnemyState); // BG의 상태를 변경된 EnemyState로 변하게 함

                    SetActiveSensor();
                }
                break;
        }
    }

    private void SetActiveSensor()
    {
        bool isSensorActive = (_viewModel.EnemyState != BT_MoveableEnemyState.Approach);
        EnemySensor_Self.gameObject.SetActive(isSensorActive);
    }

    public void OnSensorChanged()
    {
        RefreshTarget();
    }

    // BG에서 상태 변경을 받기 위함
    public void RequestStateChange(BT_MoveableEnemyState newEnemyState)
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

    public void AttackTarget()
    {
        Animator_Self.SetTrigger(ATTACK_HASH);
    }

    public void OnAttackHit()
    {
        IReadOnlyList<GameObject> targets = AttackHitbox_Self.TargetList;

        for (int i = targets.Count - 1; i >= 0; i--)
        {
            GameObject target = targets[i];

            if (target == null || target.activeInHierarchy == false)
            {
                continue;
            }

            IDamageable damageable = target.GetComponent<IDamageable>();

            if (damageable != null)
            {
                Vector3 hitDirection = (target.transform.position - transform.position).normalized;

                DamageInfo damageInfo = new DamageInfo(
                    damageAmount: _viewModel.Attack
                    , hitDirection: hitDirection
                    , damageType: DamageType.None // 몬스터 데미지 타입 전달해주세요 - 김익환
                    );

                damageable.TakeDamage(damageInfo);
            }
        }
    }

    public void Died()
    {
        OnEnemyDied?.Invoke();

        if (_viewModel.EnemyState == BT_MoveableEnemyState.Dead)
        {
            Animator_Self.SetBool(IS_DEAD_HASH, true);
        }
    }

    public void TakeDamage(DamageInfo damageInfo)
    {
        if (_viewModel.EnemyState == BT_MoveableEnemyState.Dead)
        {
            return;
        }

        int damage = Mathf.RoundToInt(damageInfo.DamageAmount);

        bool isDamaged = _viewModel.TakeDamage(damage);

        if (isDamaged == false)
        {
            return;
        }

        if (_viewModel.EnemyState == BT_MoveableEnemyState.Dead)
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
            return false; // 더 물러날 타겟이 없음 (이미 마차 등)
        }

        BehaviorGraphAgent_Self.SetVariableValue("Target", newTarget);
        BehaviorGraphAgent_Self.SetVariableValue("IsTargetDetected", newTarget != _wagon);
        return true;
    }
}