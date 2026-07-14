public class ResourceHudViewModel : BaseViewModel<ResourceModel>
{

    public ResourceHudViewModel (ResourceModel model) : base(model)
    {

    }

    public int Soul { get { return _model.Soul; } }
    public int Money { get { return _model.Money; } }

    public void AddSoul(int amount)
    {
        _model.Soul += amount;
    }
    public void AddMoney(int amount)
    {
        _model.Money += amount;
    }
}
