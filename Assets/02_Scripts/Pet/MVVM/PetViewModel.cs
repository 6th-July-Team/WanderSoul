using UnityEngine;

public class PetViewModel : BaseViewModel<PetModel>
{
    public PetViewModel(PetModel model) : base(model)
    {
    }

    public float GetHp => _model.HP;

    public void SetHp(float hp)
    {
        _model.HP = hp;
    }
}
