

public class NetworkWagonService
{
    private WagonViewModel _wagonViewModel;

    public WagonViewModel GetWagonViewModel()
    {
        if (_wagonViewModel == null)
        {
            CreateWagonViewModel();
        }

        return _wagonViewModel;
    }

    private WagonViewModel CreateWagonViewModel()
    {
        WagonData wagonData = GameManager.DataTable.GetWagonData("wagon_001");

        var wagonModel = new WagonModel();
        wagonModel.Durability = wagonData.BaseHp;
        wagonModel.MoveSpeed = wagonData.BaseMoveSpeed;
        wagonModel.Name = wagonData.Name;
        wagonModel.Capacity = wagonData.BaseCapacity;

        _wagonViewModel = new WagonViewModel(wagonModel);

        return _wagonViewModel;
    }

    public void Dispose()
    {
        _wagonViewModel.Dispose();
    }
}
