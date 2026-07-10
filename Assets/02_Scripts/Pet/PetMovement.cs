using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class PetMovement : MonoBehaviour
{
    [SerializeField] private float _stopDistanceFromAnchor = 1.5f;
    [SerializeField] private float _attackStopDistance = 1.8f;
    [SerializeField] private float _destinationUpdateDistance = 0.5f;

    private NavMeshAgent _agent;

    private Vector3 _lastDestination;
    private bool _hasDestination;

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
    }

    public void Init(float moveSpeed)
    {
        _agent.speed = moveSpeed;
    }

    public void ApplyCommand(PetCommandResult result)
    {
        if (result.Target != null && result.Target.IsAlive)
        {
            MoveToTarget(result.Target);
            return;
        }

        MoveToAnchor(result);
    }

    private void MoveToTarget(ITargetable target)
    {
        float distanceSqr = (target.Position - transform.position).sqrMagnitude;
        float stopDistanceSqr = _attackStopDistance * _attackStopDistance;

        if (distanceSqr <= stopDistanceSqr)
        {
            Stop();
            return;
        }

        SetDestinationIfNeeded(target.Position);
    }

    private void MoveToAnchor(PetCommandResult result)
    {
        Vector3 destination = GetAnchorDestination(result);

        float distanceSqr = (destination - transform.position).sqrMagnitude;
        float stopDistanceSqr = _stopDistanceFromAnchor * _stopDistanceFromAnchor;

        if (distanceSqr <= stopDistanceSqr)
        {
            Stop();
            return;
        }

        SetDestinationIfNeeded(destination);
    }

    private Vector3 GetAnchorDestination(PetCommandResult result)
    {
        return result.Command switch
        {
            EPetCommand.PlayerFollow => result.AnchorPosition + GetFollowOffset(),
            EPetCommand.GuardCart => result.AnchorPosition + GetGuardOffset(),
            EPetCommand.Aggressive => transform.position,
            _ => result.AnchorPosition
        };
    }

    private Vector3 GetFollowOffset()
    {
        return new Vector3(-1.5f, 0f, -1.5f);
    }

    private Vector3 GetGuardOffset()
    {
        return new Vector3(1.5f, 0f, 0f);
    }

    private void SetDestinationIfNeeded(Vector3 destination)
    {
        if (!ShouldUpdateDestination(destination))
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

    private bool ShouldUpdateDestination(Vector3 destination)
    {
        if (!_hasDestination)
            return true;

        float distanceSqr = (destination - _lastDestination).sqrMagnitude;
        float thresholdSqr = _destinationUpdateDistance * _destinationUpdateDistance;

        return distanceSqr >= thresholdSqr;
    }
}
