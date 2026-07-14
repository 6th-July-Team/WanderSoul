using Unity.Behavior;

[BlackboardEnum]
public enum BT_MoveableEnemyState
{
    Approach,
	Chase,
	Attack,
	Dead
}

[BlackboardEnum]
public enum BT_ProjectileEnemyState
{
    Chase,
    Attack,
    Dead
}