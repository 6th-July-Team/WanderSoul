using UnityEngine;
using UnityEngine.AI;

public class PasturePetRoaming : MonoBehaviour
{
    [SerializeField] private float _roamingRadius = 10f;
    [SerializeField] private float _waitTime = 2f;

    private NavMeshAgent _agent;
    private Vector3 _startPosition;
    private float _nextMoveTime;

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _startPosition = transform.position;
    }

    private void Update()
    {
        if (!_agent.isOnNavMesh || _agent.pathPending)
        {
            return;
        }

        if (_agent.hasPath && _agent.remainingDistance > _agent.stoppingDistance)
        {
            return;
        }

        if (Time.time < _nextMoveTime)
        {
            return;
        }

        if (MoveToRandomPosition())
        {
            _nextMoveTime = Time.time + _waitTime;
        }
    }

    private bool MoveToRandomPosition()
    {
        Vector2 randomPoint = Random.insideUnitCircle * _roamingRadius;
        Vector3 targetPosition = _startPosition + new Vector3(randomPoint.x, 0f, randomPoint.y);

        if (!NavMesh.SamplePosition(targetPosition, out NavMeshHit hit, 2f, NavMesh.AllAreas))
        {
            return false;
        }

        // 현재 위치와 너무 가까운 목적지는 다시 찾습니다.
        if ((hit.position - transform.position).sqrMagnitude < 4f)
        {
            return false;
        }

        _agent.isStopped = false;
        return _agent.SetDestination(hit.position);
    }
}
