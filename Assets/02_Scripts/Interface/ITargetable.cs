
public interface ITargetable : IPositionProvider
{
    bool IsAlive { get; }
    EntityType EntityType { get; }
}

public enum EntityType
{
    Player,
    Enemy,
    Pet,
    Wagon,
}
