using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "TryExitAttack", story: "[EnemySelf] Try to exit Attack from [Target]", category: "Action", id: "c4a8ee659f1a76792b2ef11f418fe70a")]
public partial class BT_TryExitAttackAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> EnemySelf;
    [SerializeReference] public BlackboardVariable<GameObject> Target;

    protected override Status OnStart()
    {
        EnemyView view = EnemySelf.Value.GetComponent<EnemyView>();

        if(view == null)
        {
            return Status.Failure;
        }

        view.TryExitAttackState(Target?.Value);

        return Status.Success;
    }
}

