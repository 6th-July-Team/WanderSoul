public class QuestModel : BaseModel
{
    public override void PropertyChangedOnInit()
    {
        OnPropertyChanged(nameof(State));
    }

    public QuestModel(QuestData questData)
    {
        _data = questData;

    }

    private readonly QuestData _data;
    public QuestData Data
    {
        get => _data;
    }

    private QuestState _state;
    public QuestState State
    {
        get => _state;
        set
        {
            if (_state != value)
            {
                _state = value;
                OnPropertyChanged(nameof(State));
            }
        }
    }
}