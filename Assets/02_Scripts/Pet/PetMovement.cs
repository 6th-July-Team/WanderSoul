using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class PetMovement : MonoBehaviour
{
    [SerializeField] private float _stopDistanceFromAnchor = 1.5f;
    [SerializeField] private float _attackStopDistance = 1.8f;
    [SerializeField] private float _destinationUpdateDistance = 0.5f;
    [SerializeField] private float _destinationRefreshInterval = 0.1f;

    private NavMeshAgent _agent;

    private IPositionProvider _anchor;
    private ITargetable _target;


    private Vector3 _lastDestination;
    private bool _hasDestination;
    private float _refreshTimer;


    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
    }

    public void Init(string petId)
    {
        // TODO(김익환): petId로 PetData들 설정하기.
        _agent.speed = 5f;
    }

    public void ApplyCommand(PetCommandResult result)
    {
        _anchor = result.Anchor;
        _target = result.Target;

        RefreshDestination(true);
    }

    private void Update()
    {
        _refreshTimer += Time.deltaTime;

        if (_refreshTimer < _destinationRefreshInterval)
            return;

        _refreshTimer = 0f;

        RefreshDestination(false);
    }

    public void RefreshDestination(bool force)
    {
        if (_target != null && _target.IsAlive)
        {
            MoveToTarget(_target, force);
            return;
        }

        MoveToAnchor(force);
    }

    private void MoveToTarget(ITargetable target, bool force)
    {
        Vector3 targetPosition = target.Position;

        float distanceSqr = (targetPosition - transform.position).sqrMagnitude;
        float stopDistanceSqr = _attackStopDistance * _attackStopDistance;

        if (distanceSqr <= stopDistanceSqr)
        {
            Stop();
            return;
        }

        SetDestination(targetPosition, force);
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

        SetDestination(destination, force);
    }

    private void SetDestination(Vector3 destination, bool force)
    {
        if (!force && !CheckUpdateDestination(destination))
            return;

        _agent.isStopped = false;
        _agent.SetDestination(destination);

        _lastDestination = destination;
        _hasDestination = true;
    }

    public void Stop()
    {
        _agent.isStopped = true;
        _agent.ResetPath();
        _hasDestination = false;
    }

    private bool CheckUpdateDestination(Vector3 destination)
    {
        if (!_hasDestination)
            return true;

        float distanceSqr = (destination - _lastDestination).sqrMagnitude;
        float thresholdSqr = _destinationUpdateDistance * _destinationUpdateDistance;

        return distanceSqr >= thresholdSqr;
    }
}
