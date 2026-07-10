using TMPro;
using UnityEngine;

public class CorralPanel : MonoBehaviour
{
    [SerializeField] private Transform _listRoot;

    [SerializeField] private GameObject _petTemplate;
    [SerializeField] private GameObject _productTemplate;

    public void Open(MonsterCorral monsterCorral)
    {
        gameObject.SetActive(true);
        Refresh(monsterCorral);
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }

    private void Refresh(MonsterCorral monsterCorral)
    {
        ClearList();

        if (monsterCorral.MonsterNames.Count == 0)
        {
            CreateListItem(_petTemplate, "No monsters stored.");
            CreateListItem(_productTemplate, "No products in production.");

            return;
        }

        foreach (string monsterName in monsterCorral.MonsterNames)
        {
            CreateListItem(_petTemplate, monsterName);

            CreateListItem(_productTemplate, $"{monsterName}'s specialty");
        }
    }

    private void ClearList()
    {
        foreach (Transform child in _listRoot)
        {
            if (child.gameObject == _petTemplate || child.gameObject == _productTemplate)
            {
                continue;
            }

            Destroy(child.gameObject);
        }
    }

    private void CreateListItem(GameObject template, string text)
    {
        GameObject item = Instantiate(template, _listRoot);
        item.SetActive(true);
        item.GetComponentInChildren<TMP_Text>().text = text;
    }
}
