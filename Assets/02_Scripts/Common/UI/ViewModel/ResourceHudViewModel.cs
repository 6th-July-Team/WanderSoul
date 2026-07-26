public class ResourceHudViewModel : BaseViewModel<ResourceModel>
{

    public ResourceHudViewModel (ResourceModel model) : base(model)
    {

    }

    public int Money { get { return _model.Money; } }

    public void AddMoney(int amount)
    {
        _model.Money += amount;
    }
}
