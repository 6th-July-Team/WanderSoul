using TMPro;
using UnityEngine;

public class TownHallPanel : MonoBehaviour
{
    [SerializeField] private TMP_Text _villageLevelText;
    [SerializeField] private TMP_Text _reputationText;
    [SerializeField] private TMP_Text _villageNameText;

    [Header("임시 데이터")]
    [SerializeField] private string _townId = "town_lavendil";
    [SerializeField] private int _villageLevel = 1;

    private void OnEnable()
    {
        RefreshTexts();
    }

    private void RefreshTexts()
    {
        string townName = "마을 정보 없음";
        int startReputation = 0;
        string gradeName = "평판 정보 없음";

        TownData townData = GameManager.DataTable.GetTownData(_townId);

        if ( townData != null)
        {
            townName = townData.Name;
            startReputation = townData.StartReputation;

            ReputationGradeData gradeData = GameManager.DataTable.GetReputationGradeByValue(startReputation);

            if (gradeData != null)
            {
                gradeName = gradeData.Name;
            }
        }

        _villageNameText.text = townName;
        _villageLevelText.text = $"마을 레벨: {_villageLevel}";
        _reputationText.text = $"평판: {startReputation} ({gradeName})";
    }
}
