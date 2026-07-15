using System.Collections.Generic;
using UnityEngine;
public class InventoryModel : BaseModel
{
    private List<ItemSlotModel> _itemList = new List<ItemSlotModel>();

    private Dictionary<long, ItemSlotModel> _itemDic = new Dictionary<long, ItemSlotModel>();

    public List<ItemSlotModel> ItemList { get { return _itemList; } }
    public void AddItem(ItemSlotModel item)
    {
        if (item == null)
        {
            return;
        }
        _itemList.Add(item);
        _itemDic[item.ItemUniqueId] = item;
        OnPropertyChanged(nameof(ItemList));
    }

    public void RemoveItem(long ItemUniqueId)
    {
        if (_itemDic.ContainsKey(ItemUniqueId) == false)
        {
            return;
        }

        OnPropertyChanged(nameof(ItemList));
        var item = _itemDic[ItemUniqueId];
        _itemList.Remove(item);
        _itemDic.Remove(ItemUniqueId);
        OnPropertyChanged(nameof(ItemList));
    }

    public ItemSlotModel GetItem(long itemUniqueId)
    {
        if (_itemDic.ContainsKey(itemUniqueId) == false)
        {
            return null;
        }
        return _itemDic[itemUniqueId];
    }



    public override void PropertyChangedOnInit()
    {
        OnPropertyChanged(nameof(ItemList));
    }


}
