using System;
using UnityEngine;

public class PlayerViewModel : BaseViewModel<PlayerModel>
{
    PlayerStatController _statController;

    // variables for dash
    private bool _isDashCharging;

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
    public int GetDashCount => _model.DashCount;
    public float GetMaxDashCount => _model.DashMaxCount;
    public float GetDashCoolTime => _model.DashCoolTime;
    public bool IsDashCharging => _isDashCharging;

    public bool TryDash()
    {
        if (_model.DashCount <= 0)
            return false;

        _model.DashCount--;
        return true;
    }

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

        UpdateDashCount(deltaTime);
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

    private void UpdateDashCount(float deltaTime)
    {
        if (_model.DashCount == _model.DashMaxCount)
            return;

        if (_model.DashCount >= _model.DashMaxCount)
        {
            _isDashCharging = false;
            _model.DashCoolTime = _model.DashChargeTime;
            return;
        }

        _model.DashCoolTime -= Time.deltaTime;

        if (_model.DashCoolTime > 0f)
            return;

        _model.DashCount++;

        if (_model.DashCount < _model.DashMaxCount)
        {
            _model.DashCoolTime = _model.DashChargeTime;
        }
        else
        {
            _isDashCharging = false;
            _model.DashCoolTime = 0f;
        }
    }
}
