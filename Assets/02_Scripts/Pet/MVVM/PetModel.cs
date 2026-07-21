

public class PetModel : BaseModel
{
    public override void PropertyChangedOnInit()
    {
        OnPropertyChanged(nameof(HP));
    }

    private float _hp;
    public float HP
    {
        get => _hp;
        set
        {
            if (_hp == value)
                return;
            _hp = value;
            OnPropertyChanged(nameof(HP));
        }
    }
}
