

public class PetModel : BaseModel
{
    public override void PropertyChangedOnInit()
    {
        OnPropertyChanged(nameof(HP));
        OnPropertyChanged(nameof(MaxHp));
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

    private float _maxHp;
    public float MaxHp
    {
        get => _maxHp;
        set
        {
            if (_maxHp == value)
                return;
            _maxHp = value;
            OnPropertyChanged(nameof(MaxHp));
        }
    }
}
