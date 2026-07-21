using UnityEngine;

public class PetViewModel : BaseViewModel<PetModel>
{
    public PetViewModel(PetModel model) : base(model)
    {
    }

    public float GetHp => _model.HP;
    public float GetMaxHp => _model.MaxHp;

    public void SetHp(float hp)
    {
        _model.HP = hp;
    }
}
