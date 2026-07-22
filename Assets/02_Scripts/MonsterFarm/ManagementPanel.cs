using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class ManagementPanel : MonoBehaviour
{
    [Header("사육장 강화")]
    [SerializeField] private TMP_Text _corralLevelText;
    [SerializeField] private TMP_Text _corralEffectText;
    [SerializeField] private TMP_Text _corralCostText;

    [Header("소환진 강화")]
    [SerializeField] private TMP_Text _summonLevelText;
    [SerializeField] private TMP_Text _summonEffectText;
    [SerializeField] private TMP_Text _summonCostText;

    private int _corralLevel = 1;
    private int _summonLevel = 1;
    private const int _MAX_SUMMON_LEVEL = 6;

    public void Open()
    {
        gameObject.SetActive(true);
        Refresh();
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }

    public void UpgradeCorral()
    {
        _corralLevel++;
        Refresh();
    }

    public void UpgradeSummonCircle()
    {
        if (_summonLevel >= _MAX_SUMMON_LEVEL)
        {
            return;
        }

        _summonLevel++;
        Refresh();
    }

    public float GetBonusSummonChance()
    {
        return (_summonLevel - 1) * 0.1f;
    }

    private void Refresh()
    {
        _corralLevelText.text = $"레벨: {_corralLevel}";
        _corralEffectText.text = $"보관 용량 +{_corralLevel * 2}";
        _corralCostText.text = $"비용: {_corralLevel * 100}";

        int bonusChancePercent = Mathf.RoundToInt(GetBonusSummonChance() * 100f);

        _summonLevelText.text = $"레벨: {_summonLevel}";
        _summonEffectText.text = $"추가 소환 확률: {bonusChancePercent}";

        if (_summonLevel >= _MAX_SUMMON_LEVEL)
        {
            _summonCostText.text = "최대 레벨";
        }
        else
        {
            _summonCostText.text = $"비용: {_summonLevel * 100}";
        }
    }
}
