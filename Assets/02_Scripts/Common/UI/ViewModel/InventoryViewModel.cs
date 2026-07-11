using System.Collections.Generic;

public class InventoryViewModel : BaseViewModel<InventoryModel>
{
    public InventoryViewModel(InventoryModel model) : base(model)
    {
    }

    public List<ItemSlotModel> ItemList
    {
        get { return _model.ItemList; }
    }

    public void AddItem(ItemSlotModel item)
    {
        _model.AddItem(item);
    }

    public void RemoveItem(long itemUniqueId)
    {
        _model.RemoveItem(itemUniqueId);
    }

    public ItemSlotModel GetItem(long itemUniqueId)
    {
        return _model.GetItem(itemUniqueId);
    }
}