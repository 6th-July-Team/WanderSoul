using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class ConvoyHudUIView : BaseUI
{
    [SerializeField] private Slider _progressSlider;
    [SerializeField] private RectTransform _wagonIcon;
    [SerializeField] private TMP_Text _progressText;
    [SerializeField] private TMP_Text _startTownText;
    [SerializeField] private TMP_Text _arrivalTownText;
    [SerializeField] private RectTransform _sliderFillArea;

    private Wagon _wagon;

    public void SetConvoy(string stageId, Wagon wagon)
    {
        _wagon = wagon;
        RefreshTownNames(stageId);
    }

    private void RefreshTownNames(string stageId)
    {
        var stageData = GameManager.DataTable.GetStageData(stageId);

        if (stageData == null)
        {
            return;
        }

        var startTown = GameManager.DataTable.GetTownData(stageData.StartTownId);

        if (startTown != null)
        {
            _startTownText.text = startTown.Name;
        }

        var arrivalTown = GameManager.DataTable.GetTownData(stageData.ArrivalTownId);

        if (arrivalTown != null)
        {
            _arrivalTownText.text = arrivalTown.Name;
        }
    }

    private void Update()
    {
        if ( _wagon == null)
        {
            return;
        }
        RefreshProgress();
    }

    private void RefreshProgress()
    {
        float progress = 0.4f;   
        // TODO(이태영): Wagon.Progress 추가되면 _wagon.Progress로 교체
        //float progress = _wagon.Progress;

        _progressSlider.value = progress;
        _progressText.text = $"{Mathf.RoundToInt(progress * 100f)}%";

        RefreshWagonIcon(progress);
    }

    private void RefreshWagonIcon(float progress)
    {
        if (_wagonIcon == null || _sliderFillArea == null)
        {
            return;
        }

        float width = _sliderFillArea.rect.width;
        float x = width * progress;

        Vector2 pos = _wagonIcon.anchoredPosition;
        pos.x = x;
        _wagonIcon.anchoredPosition = pos;
    }
}
