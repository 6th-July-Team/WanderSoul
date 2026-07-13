using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "EnemyChangeState", story: "[EnemySelf] ChangeState [NewState]", category: "Action", id: "5f383c6c962d78c693877ea43be0b61f")]
public partial class BT_EnemyChangeStateAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> EnemySelf;
    [SerializeReference] public BlackboardVariable<BT_MoveableEnemyState> NewState;

    protected override Status OnStart()
    {
        MoveableEnemyView view = EnemySelf.Value.GetComponent<MoveableEnemyView>();

        if(view != null)
        {
            view.RequestStateChange(NewState);
            return Status.Success;
        }

        return Status.Failure;

    }

    protected override void OnEnd()
    {
    }
}

