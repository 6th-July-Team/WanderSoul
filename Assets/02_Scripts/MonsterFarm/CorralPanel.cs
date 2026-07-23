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

        if (monsterCorral.Monsters.Count == 0)
        {
            CreateListItem(_petTemplate, "보관 중인 펫이 없습니다.");
            CreateListItem(_productTemplate, "생산 중인 특산물이 없습니다.");

            return;
        }

        foreach (PetData monster in monsterCorral.Monsters)
        {
            CreateListItem(_petTemplate, monster.Name);

            CreateListItem(_productTemplate, $"{monster.Name}의 특산물");
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
