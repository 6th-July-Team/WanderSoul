using UnityEngine;
using UnityEngine.AI;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private PlayerInputHandle _inputHandle;
    [SerializeField] private NavMeshAgent _navMeshAgent;

    // TODO(김익환): 바인더 클래스를 만드는 것을 교려
    private void OnEnable()
    {
        _inputHandle.OnMoveClickEvent += SetDestination;
    }

    private void OnDisable()
    {
        _inputHandle.OnMoveClickEvent -= SetDestination;
    }

    private void SetDestination(Vector3 destination)
    {
        _navMeshAgent.SetDestination(destination);
    }
}
