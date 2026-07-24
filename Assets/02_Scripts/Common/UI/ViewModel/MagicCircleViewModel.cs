public class MagicCircleViewModel : BaseViewModel<MagicCircleModel>
{
    public MagicCircleViewModel(MagicCircleModel model) : base(model) { }

    public int SummonOneCost => _model.SummonOneCost;
    public int SummonFiveCost => _model.SummonFiveCost;

    // TODO(태영): 소환 실행 - 소울 차감 + 몬스터 소환 결과 처리
    public bool TrySummon(int summonCount)
    {
        return false;
    }

    // TODO(태영): 레벨 초기화 - 능력치 카드/궁극기 리셋 예정임
    public void ResetLevel()
    {
    }
}
