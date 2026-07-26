using System;

public class PlayerViewModel : BaseViewModel<PlayerModel>
{
    PlayerStatController _statController;

    public PlayerViewModel(PlayerModel model, PlayerStatController statController) : base(model)
    {
        _statController = statController;
    }

    public float GetHp => _model.HP;
    public float GetMp => _model.MP;
    public float MaxMP => _statController.GetValue(StatType.MaxMana);
    public float MaxHP => _statController.GetValue(StatType.MaxHealth);
    public float GetExp => _model.EXP;
    public float GetMagnetRadius => _model.MagnetRadius;

    public void AddExp(float exp)
    {
        _model.EXP += exp;
    }

    public void AddHp(float hp)
    {
        _model.HP += hp;
    }

    public void ReduceHp(float hp)
    {
        _model.HP -= hp;
    }

    public void SetMp(float mp)
    {
        _model.MP = mp;
    }

    public void Update(float deltaTime)
    {
        float regenerationMP = _statController.GetValue(StatType.ManaRegeneration);
        RestoreMP(regenerationMP * deltaTime);

        float regenerationHP = _statController.GetValue(StatType.HealthRegeneration);
        RestoreHP(regenerationHP * deltaTime);
    }

    public bool TrySpendMP(float manaAmount)
    {
        if (_model.MP < manaAmount)
            return false;

        _model.MP -= manaAmount;

        return true;
    }

    public void RestoreMP(float amount)
    {
        if (amount <= 0f)
            return;

        _model.MP = Math.Min(_model.MP + amount, MaxMP);
    }

    public void RestoreHP(float amount)
    {
        if (amount <= 0f)
            return;

        _model.HP = Math.Min(_model.HP + amount, MaxHP);
    }

    public void RefillMP()
    {
        _model.MP = MaxMP;
    }
    
    public void SetMagnetRadius(float radius)
    {
        _model.MagnetRadius = radius;
    }
}
