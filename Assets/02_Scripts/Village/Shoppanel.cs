using UnityEngine;
using UnityEngine.UI;

public class ShopPanel : MonoBehaviour
{
    [SerializeField] private Button _generalShopButton;
    [SerializeField] private Button _equipmentShopButton;

    [SerializeField] private GameObject _itemGrid;
    [SerializeField] private GameObject _equipmentItemGrid;

    private void OnEnable()
    {
        ShowGeneralShop();
    }

    public void ShowGeneralShop()
    {
        _itemGrid.SetActive(true);
        _equipmentItemGrid.SetActive(false);

        _generalShopButton.image.color = Color.white;
        _equipmentShopButton.image.color = Color.gray;
    }

    public void ShowEquipmentShop()
    {
        _itemGrid.SetActive(false);
        _equipmentItemGrid.SetActive(true);

        _generalShopButton.image.color= Color.gray;
        _equipmentShopButton.image.color = Color.white;
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }
}
