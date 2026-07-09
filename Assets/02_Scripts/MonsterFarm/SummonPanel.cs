using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SummonPanel : MonoBehaviour
{
    [SerializeField] private Button _summonOneButton;
    [SerializeField] private Button _summonFiveButton;
    [SerializeField] private Button _summonTenButton;
    [SerializeField] private Transform _summonResultRoot;
    [SerializeField] private GameObject _summonResultCardTemplate;

    [SerializeField] private int _twistedSoulCount = 3;

    private const int SUMMON_ONE_COST = 1;
    private const int SUMMON_FIVE_COST = 5;
    private const int SUMMON_TEN_COST = 10;

    private readonly string[] _dummyPetNames =
    {
        "Fire Dragon",
        "Water Serpent",
        "Earth Golem",
        "Wind Sprite",
        "Thunder Beast"
    };

    private void OnEnable()
    {
        RefreshButtons();
    }

    private void RefreshButtons()
    {
        _summonOneButton.interactable = _twistedSoulCount >= SUMMON_ONE_COST;
        _summonFiveButton.interactable = _twistedSoulCount >= SUMMON_FIVE_COST;
        _summonTenButton.interactable = _twistedSoulCount >= SUMMON_TEN_COST;
    }

    public void SummonOne()
    {
        TrySummon(SUMMON_ONE_COST, 1);
    }

    public void SummonFive()
    {
        TrySummon(SUMMON_FIVE_COST, 5);
    }

    public void SummonTen()
    {
        TrySummon(SUMMON_TEN_COST, 10);
    }

    private void TrySummon(int cost, int count)
    {
        if (_twistedSoulCount < cost)
        {
            return;
        }

        _twistedSoulCount -= cost;

        ShowSummonResultCards(count);
        RefreshButtons();
    }

    private void ShowSummonResultCards(int count)
    {
        ClearResultCards();

        for (int i = 0; i < count; i++)
        {
            string petName = _dummyPetNames[Random.Range(0, _dummyPetNames.Length)];
            GameObject card = Instantiate(_summonResultCardTemplate, _summonResultRoot);
            card.SetActive(true);

            // 임시 카드 UI
            card.GetComponentInChildren<TMP_Text>().text = petName;

            Debug.Log($"Summoned: {petName}");
        }
    }

    private void ClearResultCards()
    {
        foreach (Transform child in _summonResultRoot)
        {
            if (child.gameObject == _summonResultCardTemplate)
            {
                continue;
            }
            Destroy(child.gameObject);
        }
    }


}
