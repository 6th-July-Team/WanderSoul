

public class NetworkPlayerService
{
    public PlayerStatController StatController => _statController;

    private PlayerViewModel _playerViewModel;
    private PlayerStatController _statController;

    private PlayerSkillViewModel _playerSkillViewModel;
    private PlayerOutGameViewModel _playerOutGameViewModel;

    public PlayerViewModel GetPlayerViewModel()
    {
        if (_playerViewModel == null)
        {
            CreatePlayerViewModel();
        }

        return _playerViewModel;
    }

    public PlayerSkillViewModel GetPlayerSkillViewModel()
    {
        if (_playerSkillViewModel == null)
        {
            CreatePlayerSkillViewModel();
        }

        return _playerSkillViewModel;
    }

    public PlayerOutGameViewModel GetPlayerOutGameViewModel()
    {
        if (_playerOutGameViewModel == null)
        {
            CreatePlayerOutGameViewModel();
        }

        return _playerOutGameViewModel;
    }

    public void Dispose()
    {
        _playerViewModel.Dispose();
        _statController.Dispose();

        _statController = null;
        _playerViewModel = null;
    }

    private PlayerSkillViewModel CreatePlayerSkillViewModel()
    {
        var model = new PlayerSkillModel();
        var viewModel = new PlayerSkillViewModel(model);
        _playerSkillViewModel = viewModel;

        return viewModel;
    }

    private PlayerViewModel CreatePlayerViewModel()
    {
        PlayerStatData playerStatData = GameManager.DataTable.GetPlayerStatData("player_scholar");
        _statController = new(playerStatData);

        var playerModel = new PlayerModel();
        playerModel.HP = _statController.GetValue(StatType.MaxHealth);
        playerModel.MP = _statController.GetValue(StatType.MaxMana);
        playerModel.EXP = 0f;
        playerModel.MagnetRadius = _statController.GetValue(StatType.MagnetRadius);
        playerModel.DashMaxCount = (int)_statController.GetValue(StatType.MaxDashCount);
        playerModel.DashChargeTime = _statController.GetValue(StatType.DashChargeTime);

        _playerViewModel = new PlayerViewModel(playerModel, _statController);

        return _playerViewModel;
    }

    private PlayerOutGameViewModel CreatePlayerOutGameViewModel()
    {
        var model = new PlayerOutGameModel();
        var viewModel = new PlayerOutGameViewModel(model);

        _playerOutGameViewModel = viewModel;

        return viewModel;
    }
}
