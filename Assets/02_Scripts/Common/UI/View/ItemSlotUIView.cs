using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlotUIView : MonoBehaviour
{
    [SerializeField] private Image _iconImage;
    [SerializeField] private TMP_Text _countText;


    private long _itemUniqueId;

    public long ItemUniqueId
    {
        get { return _itemUniqueId; }
    }

    // ItemModel 받아서 슬롯 표시
    public void InitSlot(ItemSlotModel item)
    {
        if (item == null)
        {
            return;
        }

        _itemUniqueId = item.ItemUniqueId;
        _countText.text = item.StackCount.ToString();

        // TODO:(태영, 07/10) DataTable 준비되면 아이콘/이름 연결
        // var itemData = GameManager.DataTable.GetItemData(item.ItemDataId);
        // if (itemData != null)
        // {
        //     아이콘 = itemData.IconPath 로 로드
        // }
    }
}
