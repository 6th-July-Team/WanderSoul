using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "BT_RefreshTarget", story: "[EnemySelf] refresh [target]", category: "Action", id: "cdd601db800ba59e8f10d5457ab1164a")]
public partial class BT_RefreshTargetAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> EnemySelf;
    [SerializeReference] public BlackboardVariable<GameObject> Target;

    protected override Status OnStart()
    {
        MoveableEnemyView view = EnemySelf.Value.GetComponent<MoveableEnemyView>();

        if(view == null)
        {
            return Status.Failure;
        }

        view.RefreshTarget();
        return Status.Success;
    }
}

