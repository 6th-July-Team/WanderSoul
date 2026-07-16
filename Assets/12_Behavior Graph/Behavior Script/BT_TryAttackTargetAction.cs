using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "TryAttackTarget", story: "[EnemySelf] try to Attack [Target]", category: "Action", id: "e9909ce309ac3ee6842b5dd24cd65d2e")]
public partial class BT_TryAttackTargetAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> EnemySelf;
    [SerializeReference] public BlackboardVariable<GameObject> Target;

    protected override Status OnStart()
    {
        EnemyView view = EnemySelf.Value.GetComponent<EnemyView>();

        if(view == null || Target.Value == null)
        {
            return Status.Failure;
        }

        view.TryEnterAttackState(Target.Value);

        return Status.Success;
    }
}

