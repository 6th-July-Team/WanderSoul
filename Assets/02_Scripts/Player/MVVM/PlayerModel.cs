

public class PlayerModel : BaseModel
{
    public override void PropertyChangedOnInit()
    {
        OnPropertyChanged(nameof(HP));
        OnPropertyChanged(nameof(MP));
    }

    private float _hp;
    public float HP
    {
        get { return _hp; }
        set
        {
            if (_hp == value)
            {
                return;
            }
            _hp = value;
            OnPropertyChanged(nameof(HP));
        }
    }

    private float _mp;
    public float MP
    {
        get { return _mp; }
        set
        {
            if (_mp == value)
            {
                return;
            }
            _mp = value;
            OnPropertyChanged(nameof(MP));
        }
    }

}
