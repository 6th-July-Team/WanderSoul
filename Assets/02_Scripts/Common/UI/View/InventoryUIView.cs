using System.Collections.Generic;
using UnityEngine;

public class InventoryUIView : BaseUI<InventoryUIView, InventoryViewModel>
{
    [SerializeField] private GameObject _slotPrefab;
    [SerializeField] private Transform _slotRoot;

    private List<ItemSlotUIView> _slotList = new List<ItemSlotUIView>();
    protected override void OnPropertyChanged(string propertyName)
    {
        if (propertyName == nameof(InventoryModel.ItemList))
        {
            RefreshSlots();
        }
    }

    private void RefreshSlots()
    {
        ClearSlots();

        var itemList = _viewModel.ItemList;
        if (itemList == null)
        {
            return;
        }

        foreach (var item in itemList)
        {
            CreateSlot(item);
        }
    }

    private void CreateSlot(ItemSlotModel item)
    {
        var slotObj = Instantiate(_slotPrefab, _slotRoot);
        if (slotObj == null)
        {
            return;
        }

        var slot = slotObj.GetComponent<ItemSlotUIView>();

        if (slot == null)
        {
            return;
        }

        slot.InitSlot(item);
        _slotList.Add(slot);
    }

    private void ClearSlots()
    {
        foreach (var slot in _slotList)
        {
            if (slot != null)
            {
                Destroy(slot.gameObject);
            }
        }
        _slotList.Clear();
    }
}
