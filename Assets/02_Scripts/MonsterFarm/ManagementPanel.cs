using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class ManagementPanel : MonoBehaviour
{
    [Header("Corral Upgrade")]
    [SerializeField] private TMP_Text _corralLevelText;
    [SerializeField] private TMP_Text _corralEffectText;
    [SerializeField] private TMP_Text _corralCostText;

    [Header("Summon Upgrade")]
    [SerializeField] private TMP_Text _summonLevelText;
    [SerializeField] private TMP_Text _summonEffectText;
    [SerializeField] private TMP_Text _summonCostText;

    private int _corralLevel = 1;
    private int _summonLevel = 1;

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
        _summonLevel++;
        Refresh();
    }

    private void Refresh()
    {
        _corralLevelText.text = $"Level: {_corralLevel}";
        _corralEffectText.text = $"Storage Capacity +{_corralLevel * 2}";
        _corralCostText.text = $"Cost: {_corralLevel * 100}";
        _summonLevelText.text = $"Level: {_summonLevel}";
        _summonEffectText.text = $"Summon Choices +{_summonLevel}";
        _summonCostText.text = $"Cost: {_summonLevel * 100}";
    }
}
