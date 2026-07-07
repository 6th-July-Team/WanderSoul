using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(PlayerInputHandle))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private PlayerInputHandle _inputHandle;
    [SerializeField] private NavMeshAgent _navMeshAgent;

    private InputBinder _inputBinder;

    private void Awake()
    {
        _inputBinder = new InputBinder(_inputHandle);
    }

    // TODO(김익환): 바인더 클래스를 만드는 것을 교려
    private void OnEnable()
    {
        _inputHandle.OnMoveClickEvent += SetDestination;

        _inputBinder.Bind();
    }

    private void OnDisable()
    {
        _inputHandle.OnMoveClickEvent -= SetDestination;

        _inputBinder.UnBind();
    }

    private void SetDestination(Vector3 destination)
    {
        _navMeshAgent.SetDestination(destination);
    }
}
