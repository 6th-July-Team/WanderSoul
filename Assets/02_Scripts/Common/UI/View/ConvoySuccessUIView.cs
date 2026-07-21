using TMPro;
using UnityEngine;

public class ConvoySuccessUIView : BaseUI
{
    [SerializeField] private TMP_Text _clearTimeText;
    [SerializeField] private GameObject _newRecordObject;
    [SerializeField] private TMP_Text _killedMonsterText;
    [SerializeField] private TMP_Text _gainedSoulText;
    [SerializeField] private TMP_Text _goldRewardText;
    [SerializeField] private TMP_Text _reputationRewardText;
    [SerializeField] private UIButton _moveTownButton;

    protected override void OnInit()
    {
        _moveTownButton.BindOnClickButtonEvent(OnClickMoveTown);
    }

    public void SetResult(ConvoyResultModel result)
    {
        if (result == null)
        {
            return;
        }

        _clearTimeText.text = GetClearTimeText(result.ClearTime);
        _killedMonsterText.text = $"{result.KilledMonsterCount:N0}";
        _gainedSoulText.text = $"{result.GainedSoul:N0}";
        _goldRewardText.text = $"{result.GoldReward:N0}";
        _reputationRewardText.text = $"+{result.ReputationReward}";

        if (_newRecordObject != null)
        {
            _newRecordObject.SetActive(result.IsNewRecord);
        }
    }

    private string GetClearTimeText(float clearTime)
    {
        int minutes = (int)(clearTime / 60f);
        int seconds = (int)(clearTime % 60f);
        return $"{minutes:D2}:{seconds:D2}";
    }

    private void OnClickMoveTown()
    {
        GameManager.UI.CloseUI(UIType.ConvoySuccessUIView);
        GameManager.Instance.EndConvoy();
    }
}

