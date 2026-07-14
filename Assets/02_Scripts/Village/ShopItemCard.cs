using UnityEngine;

public class ShopItemCard : MonoBehaviour
{
    [SerializeField] private string _itemName;
    [SerializeField] private int _itemPrice;

    public void Buy()
    {
        Debug.Log($"Bought {_itemName} for {_itemPrice} gold.");
    }
}
