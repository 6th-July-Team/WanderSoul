
public class PlayerOutGameViewModel : BaseViewModel<PlayerOutGameModel>
{
    public PlayerOutGameViewModel(PlayerOutGameModel model) : base(model)
    {
    }

    public int GetSoul => _model.Soul;
    public int GetGold => _model.Gold;
    public float GetExp => _model.Exp;

    public void AddSoul(int soulAmount)
    {
        _model.Soul += soulAmount;
    }

    public void ReduceSoul(int soulAmount)
    {
        _model.Soul -= soulAmount;
    }

    public void AddGold(int goldAmount)
    {
        _model.Gold += goldAmount;
    }

    public void ReduceGold(int goldAmount)
    {
        _model.Gold -= goldAmount;
    }

    public void AddExp(float expAmount)
    {
        _model.Exp += expAmount;
    }
}
