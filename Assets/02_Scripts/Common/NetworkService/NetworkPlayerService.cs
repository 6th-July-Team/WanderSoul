using UnityEngine;

public class NetworkPlayerService
{
    public PlayerStatController StatController => _statController;

    private PlayerViewModel _playerViewModel;
    private PlayerStatController _statController;


    public PlayerViewModel GetPlayerViewModel()
    {
        if (_playerViewModel == null)
        {
            CreatePlayerViewModel();
        }

        return _playerViewModel;
    }

    private PlayerViewModel CreatePlayerViewModel()
    {
        PlayerStatData playerStatData = new();//GameManager.DataTable.GetPlayerStatData("테스트 직업 아이디");
        _statController = new(playerStatData);

        var playerModel = new PlayerModel();
        playerModel.HP = _statController.GetValue(StatType.MaxHealth);
        playerModel.MP = _statController.GetValue(StatType.MaxMana);

        _playerViewModel = new PlayerViewModel(playerModel, _statController);

        return _playerViewModel;
    }

    public void Dispose()
    {
        _playerViewModel.Dispose();
    }
}
