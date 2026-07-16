using Unity.Behavior;


[BlackboardEnum]
public enum BT_EnemyState
{
    Approach,   // 이동형 전용
    Chase,      // 이동형 전용
    Attack,
    Dead,
    Idle        // 고정형 전용
}
