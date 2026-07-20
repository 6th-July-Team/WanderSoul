using Cysharp.Threading.Tasks;
using System;

public class ManaPool
{
    public PlayerStatController _statController;
    public float CurrentMana { get; private set; }
    public float MaxMana => _statController.GetValue(StatType.MaxMana);
    public bool IsManaFull => CurrentMana >= MaxMana;

    public ManaPool(PlayerStatController statController)
    {
        _statController = statController;
        CurrentMana = MaxMana;
    }

    public bool TrySpendMana(float manaAmount)
    {
        if(CurrentMana < manaAmount)
            return false;

        CurrentMana -= manaAmount;

        return true;
    }

    public void Update(float deltaTime)
    {
        float regeneration = _statController.GetValue(StatType.ManaRegeneration);

        Restore(regeneration * deltaTime);
    }

    public void Restore(float amount)
    {
        if (amount <= 0f)
            return;

        CurrentMana = Math.Min(CurrentMana + amount, MaxMana);
    }

    public void Refill()
    {
        CurrentMana = MaxMana;
    }
}
