using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class ConvoyHudUIView : BaseUI<ConvoyHudUIView, WagonViewModel>
{
    [SerializeField] private Slider _progressSlider;
    [SerializeField] private RectTransform _wagonIcon;
    [SerializeField] private TMP_Text _progressText;
    [SerializeField] private TMP_Text _startTownText;
    [SerializeField] private TMP_Text _arrivalTownText;
    [SerializeField] private RectTransform _sliderFillArea;

    [SerializeField] private UISlideAnimation _slideAnimation;

    protected override void OnOpened()
    {
        _slideAnimation.SetHidden();
        _slideAnimation.SlideIn();
    }

    public void SetConvoy(string questId)
    {
        RefreshTownNames(questId);
    }

    private void RefreshTownNames(string questId)
    {
        var questData = GameManager.DataTable.GetQuestData(questId);

        if (questData == null)
        {
            return;
        }

        var startTown = GameManager.DataTable.GetTownData(questData.StartTownId);

        if (startTown != null)
        {
            _startTownText.text = startTown.Name;
        }

        var arrivalTown = GameManager.DataTable.GetTownData(questData.ArrivalTownId);

        if (arrivalTown != null)
        {
            _arrivalTownText.text = arrivalTown.Name;
        }
    }

    protected override void OnPropertyChanged(string propertyName)
    {
        if (propertyName == nameof(WagonModel.Progress))
        {
            RefreshProgress();
        }
    }

    private void RefreshProgress()
    {
        if (_viewModel == null)
        {
            return;
        }

        float progress = _viewModel.GetProgress;

        _progressSlider.value = progress;
        _progressText.text = $"{Mathf.RoundToInt(progress * 100f)}%";
    }
}
