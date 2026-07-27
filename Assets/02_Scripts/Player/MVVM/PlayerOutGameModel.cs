
using System.Collections.Generic;

public class PlayerOutGameModel : BaseModel
{
    // TODO(이태영): 세이브 데이터가 붙으면 저장/복원 대상에 포함
    private readonly Dictionary<string, int> _levelUpPicks = new();
    public IReadOnlyDictionary<string, int> LevelUpPicks => _levelUpPicks;

    public void AddLevelUpPick(string optionId)
    {
        if (_levelUpPicks.ContainsKey(optionId) == false)
        {
            _levelUpPicks.Add(optionId, 0);
        }

        _levelUpPicks[optionId]++;
    }

    public void ClearLevelUpPicks()
    {
        _levelUpPicks.Clear();
    }

    public override void PropertyChangedOnInit()
    {
        OnPropertyChanged(nameof(Soul));
        OnPropertyChanged(nameof(Gold));
        OnPropertyChanged(nameof(Exp));
        OnPropertyChanged(nameof(Level));
        OnPropertyChanged(nameof(Reputation));
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

    private int _gold;
    public int Gold
    {
        get => _gold;
        set
        {
            if (_gold == value)
                return;
            _gold = value;
            OnPropertyChanged(nameof(Gold));
        }
    }

    private float _exp;
    public float Exp
    {
        get => _exp;
        set
        {
            if (_exp == value)
                return;
            _exp = value;
            OnPropertyChanged(nameof(Exp));
        }
    }

    private int _level = 1;
    public int Level
    {
        get => _level;
        set
        {
            if (_level == value)
                return;
            _level = value;
            OnPropertyChanged(nameof(Level));
        }
    }

    private int _reputation;
    public int Reputation
    {
        get => _reputation;
        set
        {
            if (_reputation == value)
                return;
            _reputation = value;
            OnPropertyChanged(nameof(Reputation));
        }
    }
}
