using UnityEngine;

public class TestEnemy : MonoBehaviour, ITargetable
{
    public bool IsAlive => true;

    public EntityType EntityType => EntityType.Enemy;

    public Vector3 Position => transform.position;
}
   
