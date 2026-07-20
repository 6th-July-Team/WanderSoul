using UnityEngine;

public interface IHealable
{
    bool IsHealthFull { get; }
    float Heal(float amount);
}
