using TMPro;
using UnityEngine;

public class ShopItemCard : MonoBehaviour
{
    [SerializeField] private TMP_Text _itemNameText;
    [SerializeField] private TMP_Text _itemPriceText;

    public void Buy()
    {
        Debug.Log($"Bought {_itemNameText.text} for {_itemPriceText.text} gold.");
    }
}
