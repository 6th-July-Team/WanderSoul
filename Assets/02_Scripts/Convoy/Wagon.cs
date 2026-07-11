using UnityEngine;

public class Wagon : MonoBehaviour, ITargetable
{
    public bool IsAlive => true;

    public EntityType EntityType => EntityType.Wagon;

    public Vector3 Position => transform.position;
}
