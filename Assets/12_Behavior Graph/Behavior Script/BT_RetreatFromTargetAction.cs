using System;
using Unity.Behavior;
using Unity.Properties;
using UnityEngine;
using UnityEngine.AI;
using Action = Unity.Behavior.Action;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Retreat From Target", story: "[EnemySelf] keeps preferred distance from [Target]", category: "Action", id: "4fdfa02fef4adc0109abbefd16f1d789")]
public partial class BT_RetreatFromTargetAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> EnemySelf;
    [SerializeReference] public BlackboardVariable<GameObject> Target;
    [SerializeReference] public BlackboardVariable<float> MoveSpeed;
    [SerializeReference] public BlackboardVariable<float> RepathInterval;

    private static readonly int SPEED_MAGNITUDE_HASH = Animator.StringToHash("SpeedMagnitude");

    private NavMeshAgent _navMeshAgent_enemySelf;
    private Animator _animator_enemySelf;
    private EnemyView _view;

    private float _repathTimer;

    protected override Status OnStart()
    {
        if (EnemySelf.Value == null || Target.Value == null)
        {
            return Status.Failure;
        }

        _view = EnemySelf.Value.GetComponent<EnemyView>();

        if (_view == null || _view.ShouldRetreatFromTarget(Target.Value) == false)
        {
            return Status.Failure;
        }

        _navMeshAgent_enemySelf = EnemySelf.Value.GetComponentInChildren<NavMeshAgent>();
        _animator_enemySelf = EnemySelf.Value.GetComponentInChildren<Animator>();

        if (_navMeshAgent_enemySelf == null || _navMeshAgent_enemySelf.isOnNavMesh == false)
        {
            return Status.Failure;
        }

        if (_view.TryGetRetreatPosition(Target.Value, out Vector3 retreatPosition) == false)
        {
            return Status.Failure;
        }

        _navMeshAgent_enemySelf.speed = MoveSpeed.Value;
        _navMeshAgent_enemySelf.stoppingDistance = 0f;
        _navMeshAgent_enemySelf.updateRotation = false;
        _navMeshAgent_enemySelf.SetDestination(retreatPosition);

        _repathTimer = UnityEngine.Random.Range(0f, RepathInterval.Value);

        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        if (Target.Value == null)
        {
            return Status.Failure;
        }

        UpdateAnimatorSpeed();

        if (_view.ShouldRetreatFromTarget(Target.Value) == false)
        {
            return Status.Success;
        }

        _repathTimer += Time.deltaTime;

        if (_repathTimer >= RepathInterval.Value)
        {
            _repathTimer = 0f;

            if (_view.TryGetRetreatPosition(Target.Value, out Vector3 retreatPosition))
            {
                _navMeshAgent_enemySelf.SetDestination(retreatPosition);
            }
        }

        return Status.Running;
    }

    protected override void OnEnd()
    {

        if (_navMeshAgent_enemySelf != null && _navMeshAgent_enemySelf.isOnNavMesh)
        {
            _navMeshAgent_enemySelf.updateRotation = true;
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

    private void UpdateAnimatorSpeed()
    {
        if (_animator_enemySelf == null)
        {
            return;
        }

        _animator_enemySelf.SetFloat(SPEED_MAGNITUDE_HASH, _navMeshAgent_enemySelf.velocity.magnitude);
    }
}

