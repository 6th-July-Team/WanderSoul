using UnityEngine;

public class PetStausController
{
    public bool IsAlive { get; private set; } = true;

    private float _maxHp;
    private float _currentHp;
    public float AttackPower { get; private set; } = 10f; 

    public void Init(float maxHp)
    {
        IsAlive = true;
        _maxHp = maxHp;
        _currentHp = maxHp;
    }

    public void SetAlive(bool isAlive)
    {
        IsAlive = isAlive;
    }

    public void TakeDamage(DamageInfo damageInfo)
    {
        _currentHp -= damageInfo.DamageAmount;

        if (_currentHp <= 0)
        {
            _currentHp = 0;
            IsAlive = false;
            Debug.Log("Pet is dead.");
        }
    }
}
