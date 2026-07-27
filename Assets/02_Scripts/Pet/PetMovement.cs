using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class PetMovement : MonoBehaviour
{
    [Header("Anchor Movement")]
    [SerializeField] private float _stopDistanceFromAnchor = 3f;

    [Header("Destination Refresh")]
    [SerializeField] private float _destinationUpdateDistance = 0.5f;
    [SerializeField] private float _destinationRefreshInterval = 0.1f;

    private NavMeshAgent _agent;
    private Animator _animator;

    private IPositionProvider _anchor;
    private ITargetable _target;


    private Vector3 _lastDestination;
    private bool _hasDestination;
    private float _refreshTimer;

    private float _currentStopDistance;


    private bool _isPaused;

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
    }

    public void Init(float moveSpeed, int avoidancePriority)
    {
        // TODO(김익환): petId로 PetData들 설정하기.
        _agent.speed = moveSpeed;
        _agent.avoidancePriority = avoidancePriority;
    }

    private void OnEnable()
    {
        GameManager.Time.OnPaused += PauseMovement;
        GameManager.Time.OnResumed += ResumeMovement;
    }

    private void OnDisable()
    {
        GameManager.Time.OnPaused -= PauseMovement;
        GameManager.Time.OnResumed -= ResumeMovement;
    }

    private void Update()
    {
        if (GameManager.Time.IsPaused)
            return;

        _refreshTimer += Time.deltaTime;

        if (_refreshTimer < _destinationRefreshInterval)
            return;

        _refreshTimer = 0f;

        RefreshDestination(false);
    }

    public void ApplyCommand(PetCommandResult result)
    {
        if (_isPaused)
            return;

        bool targetChanged = !ReferenceEquals(_target, result.Target);

        _anchor = result.Anchor;
        _target = result.Target;

        RefreshDestination(targetChanged);
    }

    public void SetCombatRange(float castRange)
    {
        if (_isPaused)
            return;

        if (Mathf.Approximately(_currentStopDistance, castRange))
            return;

        _currentStopDistance = castRange;

        RefreshDestination(true);
    }

    public bool IsTargetInRange(ITargetable target, float castRange)
    {
        if (!IsValidTarget(_target))
            return false;

        float sqrDistance = (target.Position - transform.position).sqrMagnitude;

        return sqrDistance <= castRange * castRange;
    }

    public void RefreshDestination(bool force)
    {
        if (_isPaused)
            return;

        if (IsValidTarget(_target))
        {
            MoveToTarget(force);
            return;
        }

        _target = null;

        MoveToAnchor(force);
    }

    private void MoveToTarget(bool force)
    {
        Vector3 targetPosition = _target.Position;

        float distanceSqr = (targetPosition - transform.position).sqrMagnitude;

        if (distanceSqr <= _currentStopDistance * _currentStopDistance)
        {
            Stop();
            return;
        }

        _animator.speed = 1f;
        SetDestination(targetPosition, _currentStopDistance, force);
    }

    private void MoveToAnchor(bool force)
    {
        if (_anchor == null)
        {
            Stop();
            return;
        }

        Vector3 destination = _anchor.Position;

        float distanceSqr = (destination - transform.position).sqrMagnitude;
        float stopDistanceSqr = _stopDistanceFromAnchor * _stopDistanceFromAnchor;

        if (distanceSqr <= stopDistanceSqr)
        {
            Stop();
            return;
        }

        _animator.speed = 1f;
        SetDestination(destination, _stopDistanceFromAnchor, force);
    }

    /// <summary>
    /// </summary>
    /// <param name="force">강제로 목적지 설정 옵션</param>
    private void SetDestination(Vector3 destination, float stoppingDistance, bool force)
    {
        if (_isPaused)
            return;

        if (!IsAgentReady())
        {
            _hasDestination = false;
            return;
        }

        if (!force && !CheckUpdateDestination(destination))
            return;

        _agent.stoppingDistance = stoppingDistance;
        _agent.isStopped = false;
        _agent.SetDestination(destination);

        _lastDestination = destination;
        _hasDestination = true;
    }

    public void Stop()
    {
        if (IsAgentReady())
        {
            _agent.isStopped = true;
            _agent.ResetPath();
        }

        _hasDestination = false;

        if(_animator != null)
            _animator.speed = 0f;
    }

    private bool CheckUpdateDestination(Vector3 destination)
    {
        if (!_hasDestination)
            return true;

        float distanceSqr = (destination - _lastDestination).sqrMagnitude;
        float thresholdSqr = _destinationUpdateDistance * _destinationUpdateDistance;

        return distanceSqr >= thresholdSqr;
    }

    private bool IsValidTarget(ITargetable target) => target != null && target.IsAlive;

    private bool IsAgentReady()
    {
        return _agent != null && _agent.enabled && _agent.isActiveAndEnabled && _agent.isOnNavMesh;
    }

    private void PauseMovement()
    {
        _isPaused = true;

        if (IsAgentReady())
        {
            _agent.isStopped = true;
        }

        if (_animator != null)
        {
            _animator.speed = 0f;
        }
    }

    private void ResumeMovement()
    {
        if (!_isPaused)
            return;

        _isPaused = false;

        RefreshDestination(true);
    }
}
