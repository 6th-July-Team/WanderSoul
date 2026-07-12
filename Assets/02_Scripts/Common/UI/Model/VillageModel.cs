using UnityEngine;

public class VillageModel : BaseModel
{
    private string _townDataId;
    public string TownDataId
    {
        get { return _townDataId; }
        set
        {
            if (_townDataId == value)
            {
                return;
            }
            _townDataId = value;
            OnPropertyChanged(nameof(TownDataId));
        }
    }

    private int _currentReputation;
    public int CurrentReputation
    {
        get { return _currentReputation; }
        set
        {
            if (_currentReputation == value)
            {
                return;
            }
            _currentReputation = value;
            OnPropertyChanged(nameof(CurrentReputation));
        }
    }
    public override void PropertyChangedOnInit()
    {
        OnPropertyChanged(nameof(TownDataId));
        OnPropertyChanged(nameof(CurrentReputation));
    }
}
