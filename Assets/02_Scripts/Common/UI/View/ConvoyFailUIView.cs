using TMPro;
using UnityEngine;

public class ConvoyFailUIView : BaseUI
{
    [SerializeField] private TMP_Text _failReasonText;
    [SerializeField] private TMP_Text _reputationPenaltyText;
    [SerializeField] private TMP_Text _repairCostText;
    [SerializeField] private GameObject _extraPenaltyObject;
    [SerializeField] private TMP_Text _extraPenaltyText;
    [SerializeField] private UIButton _returnTownButton;

    protected override void OnInit()
    {
        _returnTownButton.BindOnClickButtonEvent(OnClickReturnTown);
    }

    protected override void OnOpened()
    {
        GameManager.Time.OnPause();
    }

    protected override void OnClosed()
    {
        GameManager.Time.OnResume();
    }

    public void SetResult(ConvoyResultModel result)
    {
        if (result == null)
        {
            return;
        }

        _failReasonText.text = GetFailReasonText(result.FailReason);
        _reputationPenaltyText.text = $"-{result.ReputationPenalty}";
        _repairCostText.text = $"{result.RepairCost:N0}";

        RefreshExtraPenalty(result);
    }

    private void RefreshExtraPenalty(ConvoyResultModel result)
    {
        bool isUnpaid = (result.IsRepairCostPaid == false);

        if (_extraPenaltyObject != null)
        {
            _extraPenaltyObject.SetActive(isUnpaid);
        }

        if (isUnpaid == true && _extraPenaltyText != null)
        {
            _extraPenaltyText.text = $"-{result.ExtraReputationPenalty}";
        }
    }

    private string GetFailReasonText(ConvoyFailReason reason)
    {
        if (reason == ConvoyFailReason.PlayerDefeated)
        {
            return "당신은 끝까지 마차를 지키다 쓰러지고 말았다...";
        }
        else if (reason == ConvoyFailReason.WagonDestroyed)
        {
            return "당신은 파괴된 마차가 약탈당하는 사이 겨우 도망쳤다...";
        }
        else if (reason == ConvoyFailReason.OutOfWagonArea)
        {
            return "당신은 몬스터가 몰려들자 마차를 버리고 도망쳤다...";
        }

        return string.Empty;
    }

    private void OnClickReturnTown()
    {
        GameManager.UI.CloseUI(UIType.ConvoyFailUIView);
        GameManager.Instance.EndConvoy();
    }
}

