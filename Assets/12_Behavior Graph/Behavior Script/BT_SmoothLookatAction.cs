using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "SmoothLookat", story: "[EnemySelf] Smoothly looks at [Target]", category: "Action", id: "374f0d85891843e56320e7c3732767c0")]
public partial class BT_SmoothLookatAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> EnemySelf;
    [SerializeReference] public BlackboardVariable<GameObject> Target;
    [SerializeReference] public BlackboardVariable<float> RotateSpeed;

    private const float COMPLETE_ANGLE = 15f;
    private const float TIME_OUT = 1.5f;
    private float _elapsedTime;

    private Transform _enemySelfTransform;
    private Transform _targetTransform;

    protected override Status OnStart()
    {
        if (EnemySelf.Value == null || Target.Value == null)
        {
            return Status.Failure;
        }

        _elapsedTime = 0f;

        _enemySelfTransform = EnemySelf.Value.transform;
        _targetTransform = Target.Value.transform;

        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        if (EnemySelf.Value == null || Target.Value == null)
        {
            return Status.Failure;
        }

        _elapsedTime += Time.deltaTime;

        if(_elapsedTime >= TIME_OUT)
        {
            return Status.Success;
        }

        Vector3 direction = _targetTransform.position - _enemySelfTransform.position;
        direction.y = 0f;

        if(direction.sqrMagnitude < 0.0001f)
        {
            return Status.Success;
        }

        Quaternion targetRotation = Quaternion.LookRotation(direction);

        _enemySelfTransform.rotation = Quaternion.RotateTowards(_enemySelfTransform.rotation, targetRotation, RotateSpeed.Value * Time.deltaTime);

        float remainingAngle = Quaternion.Angle(_enemySelfTransform.rotation, targetRotation);

        if(remainingAngle <= COMPLETE_ANGLE)
        {
            return Status.Success;
        }

        return Status.Running;
    }
}

