using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "EnemyDied", story: "[EnemySelf] is Died", category: "Action", id: "b3e94750d9d3b1a1fb83ecb0589e7064")]
public partial class BT_EnemyDiedAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> EnemySelf;

    protected override Status OnStart()
    {
        EnemyView view = EnemySelf.Value.GetComponent<EnemyView>();

        if(view == null)
        {
            return Status.Failure;
        }

        view.PlayDeadAction();

        return Status.Success;
    }
}

