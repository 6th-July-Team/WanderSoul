public class QuestViewModel : BaseViewModel<QuestModel>
{
    public string Name => _model.Data.Name;
    public string Description => _model.Data.Description;
    public QuestType Type => _model.Data.Type;
    public int Difficulty => _model.Data.Difficulty;
    public int GoldReward => _model.Data.GoldReward;
    public int ReputationReward => _model.Data.ReputationReward;
    public string StartTownId => _model.Data.StartTownId;
    public string ArrivalTownId => _model.Data.ArrivalTownId;
    public QuestState QuestState => _model.State;

    public QuestViewModel(QuestModel model) : base(model) { }

    private bool CanAcceptQuest(int currentReputation)
    {
        if (_model.State != QuestState.NotStarted)
        {
            return false;
        }

        bool canAccept = currentReputation >= _model.Data.RequiredReputation;

        return canAccept;
    }

    public bool TryAcceptQuest(int currentReputation)
    {
        if (CanAcceptQuest(currentReputation) == false)
        {
            return false;
        }

        _model.State = QuestState.InProgress;
        return true;
    }

    public void Complete()
    {
        if (_model.State != QuestState.InProgress)
        {
            return;
        }

        _model.State = QuestState.Completed;
        return;
    }

    public void Fail()
    {
        if (_model.State != QuestState.InProgress)
        {
            return;
        }

        _model.State = QuestState.Failed;
        return;
    }
}
