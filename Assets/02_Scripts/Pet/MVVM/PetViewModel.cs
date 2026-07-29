using UnityEngine;

public class PetViewModel : BaseViewModel<PetModel>
{
    public PetViewModel(PetModel model) : base(model)
    {
    }

    public float GetHp => _model.HP;
    public float GetMaxHp => _model.MaxHp;

    public void ReduceHp(float damage)
    {
        _model.HP -= damage;
        if (_model.HP < 0)
            _model.HP = 0;
    }

    public void SetHp(float hp)
    {
        _model.HP = hp;
    }
}
