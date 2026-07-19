
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
    Barrier,
}

public enum TargetType
{
    Player,
    Pet,
    Wagon,
    PlayerAndPet,
    Ally,           // 유저 기준 아군
    Enemy,
    All,
}
