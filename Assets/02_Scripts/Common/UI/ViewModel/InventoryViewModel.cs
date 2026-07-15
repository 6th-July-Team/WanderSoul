using System.Collections.Generic;

public class InventoryViewModel : BaseViewModel<InventoryModel>
{
    private long _selectedItemId;
    public InventoryViewModel(InventoryModel model) : base(model)
    {
    }

    public List<ItemSlotModel> ItemList
    {
        get { return _model.ItemList; }
    }

    public ItemSlotModel SelectedItem
    {
        get { return _model.GetItem(_selectedItemId); }
    }

    public void SelectItem(long itemUniqueId)
    {
        if (_selectedItemId == itemUniqueId)
        {
            return;
        }

        _selectedItemId = itemUniqueId;
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