using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CorralPanel : MonoBehaviour
{
    [SerializeField] private Transform _petListRoot;
    [SerializeField] private Transform _productListRoot;

    [SerializeField] private TMP_Text _petEmptyText;
    [SerializeField] private TMP_Text _productEmptyText;

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
        ClearList(_petListRoot, _petTemplate);
        ClearList(_productListRoot, _productTemplate);

        bool isEmpty = monsterCorral.Monsters.Count == 0;

        _petEmptyText.gameObject.SetActive(isEmpty);
        _productEmptyText.gameObject.SetActive(isEmpty);

        if (isEmpty)
        {
            return;
        }

        foreach (PetData monster in monsterCorral.Monsters)
        {
            string petText = $"{monster.Name} / 능력치 정보 없음";

            PetStatData statData = GameManager.DataTable.GetPetStatData(monster.Id);

            if (statData != null)
            {
                petText = $"{monster.Name}\nHP {statData.MaxHealth:0}\n 이동속도 {statData.MoveSpeed:0.#}";
            }

            CreatePetListItem(monster, petText);
            CreateListItem(_productTemplate, _productListRoot, $"{monster.Name}\n특산물");
        }
    }

    private void CreatePetListItem(PetData monster, string text)
    {
        GameObject item = Instantiate(_petTemplate, _petListRoot);
        item.SetActive(true);

        TMP_Text infoText = item.GetComponentInChildren<TMP_Text>();
        infoText.text = text;

        Transform iconTransform = item.transform.Find("IconImage");

        if (iconTransform == null)
        {
            return;
        }

        Image iconImage = iconTransform.GetComponent<Image>();
        Sprite iconSprite = Utils.ResourcesLoad<Sprite>(monster.IconPath);

        if (iconSprite != null)
        {
            iconImage.sprite = iconSprite;
            iconImage.preserveAspect = true;
        }
    }

    private void CreateListItem(GameObject template, Transform listRoot, string text)
    {
        GameObject item = Instantiate(template, listRoot);
        item.SetActive(true);
        item.GetComponentInChildren<TMP_Text>().text = text;
    }

    private void ClearList(Transform listRoot, GameObject template)
    {
        foreach (Transform child in listRoot)
        {
            if (child.gameObject == template)
            {
                continue;
            }

            Destroy(child.gameObject);
        }
    }
}
