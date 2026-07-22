using System.Collections.Generic;
using UnityEngine;

public class NetworkPlayerService
{
    public PlayerStatController StatController => _statController;

    private PlayerViewModel _playerViewModel;
    private PlayerStatController _statController;

    private PlayerSkillViewModel _playerSkillViewModel;

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

    public void Dispose()
    {
        _playerViewModel.Dispose();
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
        PlayerStatData playerStatData = new();//GameManager.DataTable.GetPlayerStatData("테스트 직업 아이디");
        _statController = new(playerStatData);

        var playerModel = new PlayerModel();
        playerModel.HP = _statController.GetValue(StatType.MaxHealth);
        playerModel.MP = _statController.GetValue(StatType.MaxMana);

        _playerViewModel = new PlayerViewModel(playerModel, _statController);

        return _playerViewModel;
    }
}
