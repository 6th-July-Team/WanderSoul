using TMPro;
using UnityEngine;

public class TownHallUIView : BaseUI<TownHallUIView, VillageInfoViewModel>
{
    [SerializeField] private TMP_Text _villageNameText;
    [SerializeField] private TMP_Text _villageLevelText;
    [SerializeField] private TMP_Text _reputationText;
    [SerializeField] private UIButton _closeButton;

    protected override void OnInit()
    {
        if (_closeButton != null)
        {
            _closeButton.BindOnClickButtonEvent(OnclickClose);
        }
    }

    protected override void OnPropertyChanged(string propertyName)
    {
        if (propertyName == nameof(VillageModel.TownDataId) || propertyName == nameof(VillageModel.VillageLevel) || propertyName == nameof(VillageModel.CurrentReputation))
        {
            RefreshTexts();
        }
    }

    private void RefreshTexts()
    {
        _villageNameText.text = _viewModel.TownName;
        _villageLevelText.text = $"마을 레벨: {_viewModel.VillageLevel}";
        _reputationText.text = $"평판: {_viewModel.CurrentReputation} ({_viewModel.ReputationGradeName})";
    }

    private void OnclickClose()
    {
        GameManager.UI.CloseUI(UIType.TownHallUIView);
    }
}
