using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using UnityEngine.AI;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Navigates to Target", story: "[EnemySelf] Navigates to [Target]", category: "Action", id: "ae19bd74860c77d30fd3160bcb074737")]
public partial class BT_NavigatesToTargetAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> EnemySelf; // 적 자기 자신
    [SerializeReference] public BlackboardVariable<GameObject> Target; // 타겟
    [SerializeReference] public BlackboardVariable<float> MoveSpeed; // 이동 속도
    [SerializeReference] public BlackboardVariable<float> StopDistance; // 정지 거리
    [SerializeReference] public BlackboardVariable<float> RepathInterval; // 경로 재계산 최소 시간
    
    private static readonly int SPEED_MAGNITUDE_HASH = Animator.StringToHash(SPEED_PARAM); // SPEED_PARAM를 Animator에 전달해주기 위한 Hash

    private const float REPATH_DISTANCE = 0.5f; // 경로 재계산 최소 거리
    private const string SPEED_PARAM = "SpeedMagnitude"; // Animator의 이동 속도 파라미터

    private const float PROGRESS_EPSILON = 0.25f; // 이만큼도 목적지에 가까워지지 못하면 진전 없음으로 간주 (군중 속에서 옆으로 밀리는 건 이동으로 안 침)
    private const float STUCK_TIME = 1.5f; // 이 시간 동안 진전이 없으면 대기 모드로 변경

    private const float WAIT_TIME = 2f; // 대기 시간 ( 지나면 재시도 )

    private NavMeshAgent _navMeshAgent_enemySelf; // 적 자기 자신의 NavMeshAgent
    private Animator _animator_enemySelf; // 적 자기자신의 Animator
    
    private MoveableEnemyView _view;

    private Vector3 _lastRepathPosition; // 마지막으로 계산한 타겟의 위치

    private float _repathTimer; // 마지막 경로 재계산 후 지나간 시간
    private float _stuckTimer; // 진전 없이 흐른 시간
    private float _waitTimer; // 남은 대기 시간 (0보다 크면 대기 모드)

    private float _bestRemainingDistance; // 지금까지 도달한 목적지까지의 최소 남은 거리 (진전 판정 기준)

    // 노드 시작 시 컴포넌트 캐싱 및 이동/타이머 초기화
    protected override Status OnStart()
    {
        if (EnemySelf.Value == null || Target.Value == null)
        {
            return Status.Failure;
        }

        _view = EnemySelf.Value.GetComponent<MoveableEnemyView>();

        _navMeshAgent_enemySelf = EnemySelf.Value.GetComponentInChildren<NavMeshAgent>();
        _animator_enemySelf = EnemySelf.Value.GetComponentInChildren<Animator>();

        if (_navMeshAgent_enemySelf == null || _navMeshAgent_enemySelf.isOnNavMesh == false)
        {
            return Status.Failure;
        }

        _navMeshAgent_enemySelf.speed = MoveSpeed.Value;
        _navMeshAgent_enemySelf.stoppingDistance = StopDistance.Value;

        _lastRepathPosition = Target.Value.transform.position;

        _navMeshAgent_enemySelf.SetDestination(GetDestination());

        _repathTimer = UnityEngine.Random.Range(0f, RepathInterval.Value); // 계산해야 되는 객체가 많을 때 모든 객체가 한 프레임에 일제히 재계산하는 것을 방지하기 위해 Random으로 만듬
        _stuckTimer = 0f;
        _waitTimer = 0f;
        _bestRemainingDistance = float.MaxValue;

        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        if (Target.Value == null)
        {
            return Status.Failure;
        }

        UpdateAnimatorSpeed();

        if (IsArrived())
        {
            return Status.Success;
        }

        if (_waitTimer > 0f)                            // 대기 모드: 밀지 않고 가만히
        {
            _waitTimer -= Time.deltaTime;

            if (_waitTimer <= 0f)
            {
                _navMeshAgent_enemySelf.isStopped = false;   // 대기 끝 — 재시도 (앞줄이 죽어 빈자리가 났을 수 있음)
            }

            return Status.Running;
        }

        UpdateStuckState();

        TryRepath();

        return Status.Running;
    }

    // 노드 종료 시 navMeshAgent가 있고, navMesh 위에 있을 경우 ResetPath(=경로 초기화) 및 애니메이터가 있을 경우 Animator의 SpeedMagnitude(=이동 속도)값을 0으로 만든 후, navMeshAgent와 Animator를 초기화함
    protected override void OnEnd()
    {
        if (_navMeshAgent_enemySelf != null && _navMeshAgent_enemySelf.isOnNavMesh)
        {
            _navMeshAgent_enemySelf.isStopped = false;

            _navMeshAgent_enemySelf.ResetPath();
        }

        if (_animator_enemySelf != null)
        {
            _animator_enemySelf.SetFloat(SPEED_MAGNITUDE_HASH, 0f);
        }

        _navMeshAgent_enemySelf = null;
        _animator_enemySelf = null;
        _view = null;
    }

    // _navMeshAgent_enemySelf가 pathPending(=경로 탐색) 중인지, reminingDistance(=목표와의 거리)가 stoppingDistance(=멈춰야 하는 거리)와 같거나 작은지 체크해서 bool값을 리턴하는 메서드
    private bool IsArrived()
    {
        if (_navMeshAgent_enemySelf.pathPending) // SetDestination 직후에 경로 계산이 끝나기 전까지는 remainingDistance가 부정확하므로 도착 판정을 보류함
        {
            return false;
        }

        bool isArrived = _navMeshAgent_enemySelf.remainingDistance <= _navMeshAgent_enemySelf.stoppingDistance;

        return isArrived;
    }

    // 목적지까지의 남은 거리가 STUCK_TIME 동안 줄어들지 않으면(속도가 아니라 "진전" 기준 — 군중에 밀려 옆으로 흔들리는 것은 진전이 아님)
    // 밀기를 포기하고 잠시 대기 모드로 전환하는 메서드. 대기가 끝나면 재시도함 (앞줄이 죽어 빈자리가 났을 수 있음)
    private void UpdateStuckState()
    {
        if (_navMeshAgent_enemySelf.pathPending) // 경로 계산 중엔 remainingDistance가 부정확하므로 판정 보류
        {
            return;
        }

        float remainingDistance = _navMeshAgent_enemySelf.remainingDistance;

        if (remainingDistance < _bestRemainingDistance - PROGRESS_EPSILON) // 목적지에 실제로 가까워졌는가?
        {
            _bestRemainingDistance = remainingDistance;
            _stuckTimer = 0f;
            return;
        }

        _stuckTimer += Time.deltaTime;

        if (_stuckTimer < STUCK_TIME)
        {
            return;
        }

        _stuckTimer = 0f;
        _bestRemainingDistance = float.MaxValue; // 새 타겟이든 재시도든 진전을 새로 측정

        if (_view != null && _view.TryFallbackTarget()) // 1차: 후순위 타겟으로 전환 시도
        {
            // 전환 성공 — 멈추지 않고 새 타겟으로 즉시 재출발
            _lastRepathPosition = Target.Value.transform.position;
            _navMeshAgent_enemySelf.SetDestination(GetDestination());
            return;
        }

        // 2차: 더 물러날 타겟이 없음(이미 마차 등) — 기존 대기 로직으로 폴백
        _navMeshAgent_enemySelf.isStopped = true;
        _navMeshAgent_enemySelf.velocity = Vector3.zero;
        _waitTimer = WAIT_TIME + UnityEngine.Random.Range(0f, 1f);
    }

    // RepathInterval(=재계산 주기)가 지났고, Target이 REPATH_DISTANCE(=경로 재계산 거리) 이상 움직였을 때만 경로를 재계산하는 메서드
    private void TryRepath()
    {
        _repathTimer += Time.deltaTime;

        if (_repathTimer < RepathInterval.Value)
        {
            return;
        }

        _repathTimer = 0f;

        Vector3 targetPosition = Target.Value.transform.position;
        float moveSqr = (targetPosition - _lastRepathPosition).sqrMagnitude;

        if (moveSqr < REPATH_DISTANCE * REPATH_DISTANCE)
        {
            return;
        }

        _lastRepathPosition = targetPosition;
        _navMeshAgent_enemySelf.SetDestination(GetDestination());
    }

    // Animator의 SpeedMagnitude(=이동 속도 값) 파라미터를 _navMeshAgent의 velocity의 길이로     갱신해주는 메서드
    private void UpdateAnimatorSpeed()
    {
        if (_animator_enemySelf == null)
        {
            return;
        }

        _animator_enemySelf.SetFloat(SPEED_MAGNITUDE_HASH, _navMeshAgent_enemySelf.velocity.magnitude);
    }

    // 타겟 콜라이더 표면에서 자신과 가장 가까운 점을 목적지로 반환하는 메서드
    // (마차처럼 큰 대상은 중심점이 도달 불가능한 지점이므로 표면 기준으로 이동)
    private Vector3 GetDestination()
    {
        Vector3 selfPosition = EnemySelf.Value.transform.position;

        if (Target.Value.TryGetComponent(out Collider targetCollider))
        {
            return targetCollider.ClosestPoint(selfPosition);
        }

        return Target.Value.transform.position;
    }
}

