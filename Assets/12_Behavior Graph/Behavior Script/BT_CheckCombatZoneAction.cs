using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "CheckCombatZone", story: "[EnemySelf] checks combat zone [Wagon]", category: "Action", id: "ab4fd1979152c12485aa369181247942")]
public partial class BT_CheckCombatZoneAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> EnemySelf;
    [SerializeReference] public BlackboardVariable<GameObject> Wagon;
    [SerializeReference] public BlackboardVariable<float> CombatReadyRange;

    protected override Status OnStart()
    {
        if (EnemySelf.Value == null || Wagon.Value == null)
        {
            return Status.Failure;
        }

        float distanceSqr = (Wagon.Value.transform.position - EnemySelf.Value.transform.position).sqrMagnitude;
        float rangeSqr = CombatReadyRange.Value * CombatReadyRange.Value;

        if (distanceSqr <= rangeSqr)
        {
            MoveableEnemyView view = EnemySelf.Value.GetComponent<MoveableEnemyView>();
            
            if(view == null)
            {
                return Status.Failure;
            }

            view.RequestStateChange(BT_MoveableEnemyState.Chase);
        }

        return Status.Success;
    }
}

