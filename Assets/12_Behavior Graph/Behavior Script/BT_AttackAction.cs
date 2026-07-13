using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Attack", story: "[EnemySelf] attack [Target]", category: "Action", id: "d85ec8d3147aef02a9b67b2410369b22")]
public partial class BT_AttackAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> EnemySelf;
    [SerializeReference] public BlackboardVariable<GameObject> Target;

    protected override Status OnStart()
    {
        IEnemyView view = EnemySelf.Value.GetComponent<IEnemyView>();

        if(view == null)
        {
            return Status.Failure;
        }

        view.AttackTarget();

        return Status.Success;
    }
}

