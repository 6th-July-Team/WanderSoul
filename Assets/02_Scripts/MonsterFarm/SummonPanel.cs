using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class SummonPanel : MonoBehaviour
{
    [SerializeField] private Button _summonOneButton;
    [SerializeField] private Button _summonFiveButton;
    [SerializeField] private Button _summonTenButton;
    [SerializeField] private Button _rerollButton;

    [SerializeField] private Transform _summonResultRoot;
    [SerializeField] private GameObject _summonResultCardTemplate;
    [SerializeField] private MonsterCorral _monsterCorral;

    [SerializeField] private SOPetDefinition[] _PetDefinitions;

    [SerializeField] private int _twistedSoulCount = 3;

    private const int SUMMON_ONE_COST = 1;
    private const int SUMMON_FIVE_COST = 5;
    private const int SUMMON_TEN_COST = 10;

    private int _lastSummonCount;

    private void OnEnable()
    {
        RefreshButtons();
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

    public void Reroll()
    {
        if (_lastSummonCount == 0)
        {
            return;
        }

        _monsterCorral.RemoveLastMonsters(_lastSummonCount);
        ShowSummonResultCards(_lastSummonCount);
    }

    public void Close()
    {
        _lastSummonCount = 0;
        gameObject.SetActive(false);
    }

    private void TrySummon(int cost, int count)
    {
        if (_PetDefinitions == null || _PetDefinitions.Length == 0)
        {
            return;
        }

        if (_twistedSoulCount < cost)
        {
            return;
        }

        _twistedSoulCount -= cost;
        _lastSummonCount = count;

        ShowSummonResultCards(count);
        RefreshButtons();
    }

    private void ShowSummonResultCards(int count)
    {
        ClearResultCards();

        for (int i = 0; i < count; i++)
        {
            SOPetDefinition petDefinition = _PetDefinitions[Random.Range(0, _PetDefinitions.Length)];

            _monsterCorral.AddMonster(petDefinition);

            GameObject card = Instantiate(_summonResultCardTemplate, _summonResultRoot);
            card.SetActive(true);
            card.GetComponentInChildren<TMP_Text>().text = petDefinition.Name;

            Debug.Log($"Summoned: {petDefinition.Name}");
        }
    }

    private void RefreshButtons()
    {
        _summonOneButton.interactable = _twistedSoulCount >= SUMMON_ONE_COST;
        _summonFiveButton.interactable = _twistedSoulCount >= SUMMON_FIVE_COST;
        _summonTenButton.interactable = _twistedSoulCount >= SUMMON_TEN_COST;
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
