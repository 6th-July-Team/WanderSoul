
public class PlayerOutGameViewModel : BaseViewModel<PlayerOutGameModel>
{
    public PlayerOutGameViewModel(PlayerOutGameModel model) : base(model)
    {
    }

    public int GetSoul => _model.Soul;

    public void AddSoul(int soulAmount)
    {
        _model.Soul += soulAmount;
    }
}
