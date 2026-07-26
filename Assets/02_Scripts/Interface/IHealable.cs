using UnityEngine;

public interface IHealable
{
    bool IsHealthFull { get; }
    void Heal(float amount);
}
