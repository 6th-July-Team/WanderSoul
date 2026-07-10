using UnityEngine;

public class PartyHudViewModel : BaseViewModel<PartyMemberModel>
{
    public PartyHudViewModel(PartyMemberModel model) : base(model)
    {
    }

    public int CurrentHp { get { return _model.CurrentHp; } }
    public int MaxHp { get { return _model.MaxHp; } }
    public string Name { get { return _model.Name; } }
    public float HpFillAmount { get { return _model.HpFillAmount; } }

    public void TakeDamage(int damage)
    {
        _model.CurrentHp -= damage;
    }

    public void Heal(int amount)
    {
        _model.CurrentHp += amount;
    }
}
