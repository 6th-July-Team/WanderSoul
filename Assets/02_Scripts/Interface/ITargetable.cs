using UnityEngine;

public interface ITargetable
{
    Vector3 Position { get; }
    bool IsAlive { get; }
    EntityType EntityType { get; }
}

public enum EntityType
{
    Player,
    Enemy,
    Pet,
    COUNT
}
