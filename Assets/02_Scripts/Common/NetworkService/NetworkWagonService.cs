using UnityEngine;

public class NetworkWagonService
{
    private WagonViewModel _wagonViewModel;


    public WagonViewModel GetWagonViewModel(string wagonId)
    {
        if (_wagonViewModel == null)
        {
            CreateWagonViewModel(wagonId);
        }

        return _wagonViewModel;
    }

    private WagonViewModel CreateWagonViewModel(string wagonId)
    {
        WagonData wagonData = GameManager.DataTable.GetWagonData(wagonId);

        var wagonModel = new WagonModel();
        wagonModel.Durability = wagonData.BaseHp;
        wagonModel.MoveSpeed = wagonData.BaseMoveSpeed;
        wagonModel.Name = wagonData.Name;
        wagonModel.Capacity = wagonData.BaseCapacity;

        _wagonViewModel = new WagonViewModel(wagonModel);

        return _wagonViewModel;
    }
}
