

public class PlayerOutGameModel : BaseModel
{
    public override void PropertyChangedOnInit()
    {
        OnPropertyChanged(nameof(Soul));
    }

    private int _soul;
    public int Soul
    {
        get => _soul;
        set
        {
            if (_soul == value)
                return;
            _soul = value;
            OnPropertyChanged(nameof(Soul));
        }
    }
}
