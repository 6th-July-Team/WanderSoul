public class PetSlotModel : BaseModel
{
    private long _petUniqueId;
    public long PetUniqueId
    {
        get { return _petUniqueId; }
        set
        {
            if (_petUniqueId == value) return;
            _petUniqueId = value;
            OnPropertyChanged(nameof(PetUniqueId));
        }
    }

    private string _petDataId;
    public string PetDataId
    {
        get { return _petDataId; }
        set
        {
            if (_petDataId == value) return;
            _petDataId = value;
            OnPropertyChanged(nameof(PetDataId));
        }
    }

    private int _level;
    public int Level
    {
        get { return _level; }
        set
        {
            if (_level == value) return;
            _level = value;
            OnPropertyChanged(nameof(Level));
        }
    }

    public override void PropertyChangedOnInit()
    {
        OnPropertyChanged(nameof(PetUniqueId));
        OnPropertyChanged(nameof(PetDataId));
        OnPropertyChanged(nameof(Level));
    }
}