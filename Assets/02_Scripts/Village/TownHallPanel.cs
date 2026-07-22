using TMPro;
using UnityEngine;

public class TownHallPanel : MonoBehaviour
{
    [SerializeField] private TMP_Text _villageLevelText;
    [SerializeField] private TMP_Text _upgradeCostText;

    [SerializeField] private int _villageLevel = 1;
    [SerializeField] private int _upgradeCost = 1000;
    [SerializeField] private int _upgradeCostIncresePercent = 20;

    private void OnEnable()
    {
        RefreshTexts();
    }

    public void UpgradeVillage()
    {
        _villageLevel++;

        int increaseAmount = _upgradeCost * _upgradeCostIncresePercent / 100;
        _upgradeCost += increaseAmount;

        RefreshTexts();

        Debug.Log($"Village upgraded to level {_villageLevel}");
    }

    private void RefreshTexts()
    {
        _villageLevelText.text = $"마을 레벨: {_villageLevel}";
        _upgradeCostText.text = $"업그레이드 비용: {_upgradeCost}";
    }
}
