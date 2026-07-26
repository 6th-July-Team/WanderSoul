public class MagicCircleModel : BaseModel
{
    public override void PropertyChangedOnInit()
    {
        OnPropertyChanged(nameof(SummonOneCost));
        OnPropertyChanged(nameof(SummonFiveCost));
    }

    // TODO(태영): 소환 비용 데이터 원본 정해지면 DataTable에서 로드
    private int _summonOneCost;
    public int SummonOneCost
    {
        get { return _summonOneCost; }
        set
        {
            if (_summonOneCost == value)
            {
                return;
            }
            _summonOneCost = value;
            OnPropertyChanged(nameof(SummonOneCost));
        }
    }

    private int _summonFiveCost;
    public int SummonFiveCost
    {
        get { return _summonFiveCost; }
        set
        {
            if (_summonFiveCost == value)
            {
                return;
            }
            _summonFiveCost = value;
            OnPropertyChanged(nameof(SummonFiveCost));
        }
    }
}
