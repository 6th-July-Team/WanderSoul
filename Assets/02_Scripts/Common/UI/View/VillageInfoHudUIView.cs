using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class VillageInfoHudUIView : BaseUI<VillageInfoHudUIView, VillageInfoViewModel>
{
    [SerializeField] private TMP_Text _townNameText;
    [SerializeField] private TMP_Text _gradeNameText;
    [SerializeField] private Slider _reputationSlider;
    [SerializeField] private Image _regionLogoImage;
    [SerializeField] private Image _gradeIconImage;

    protected override void OnPropertyChanged(string propertyName)
    {
        if (propertyName == nameof(VillageModel.TownDataId))
        {
            RefreshTown();
        }

        else if (propertyName == nameof(VillageModel.CurrentReputation))
        {
            RefreshReputation();
        }
    }

    private void RefreshTown()
    {
        _townNameText.text = _viewModel.TownName;

        // TODO(이태영): 이미지 준비되면 엠블럼 로드

        RefreshReputation();
    }

    private void RefreshReputation()
    {
        _gradeNameText.text = _viewModel.ReputationGradeName;
        _reputationSlider.value = _viewModel.ReputationFillAmount;

        // TODO(이태영): 이미지 준비되면 등급 아이콘 로드
    }
}
