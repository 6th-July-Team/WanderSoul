using UnityEngine;

public interface IPositionProvider
{
    Vector3 Position { get; }
    Transform Transform { get; }
}
