using UnityEngine;

public class PlayerEntity : MonoBehaviour, ITargetable
{

    public bool IsAlive => true;

    public EntityType EntityType => EntityType.Player;

    public Vector3 Position => transform.position;
}
