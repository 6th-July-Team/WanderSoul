using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlotUIView : MonoBehaviour
{
    [SerializeField] private Image _iconImage;
    [SerializeField] private TMP_Text _countText;
    [SerializeField] private UIButton _slotButton;
    [SerializeField] private Image _selectedImage;

    private long _itemUniqueId;
    private event Action<long> OnSlotSelected;

    public long ItemUniqueId
    {
        get { return _itemUniqueId; }
    }

    private void OnEnable()
    {
        _slotButton.BindOnClickButtonEvent(OnClickSlot);
        SetSelected(false);
    }

    public void InitSlot(ItemSlotModel item)
    {
        if (item == null)
        {
            return;
        }

        _itemUniqueId = item.ItemUniqueId;
        _countText.text = item.StackCount.ToString();

        RefreshIcon(item.ItemDataId);
    }

    private void RefreshIcon(string itemDataId)
    {
        var itemData = GameManager.DataTable.GetItemData(itemDataId);
        if (itemData == null)
        {
            return;
        }

        if (string.IsNullOrEmpty(itemData.IconPath))
        {
            return;
        }

        Sprite iconSprite = Utils.ResourcesLoad<Sprite>(itemData.IconPath);
        if (iconSprite != null)
        {
            _iconImage.sprite = iconSprite;
        }
    }

    public void BindSelectEvent(Action<long> onSelected)
    {
        OnSlotSelected = onSelected;
    }

    private void OnClickSlot()
    {
        if (OnSlotSelected != null)
        {
            OnSlotSelected.Invoke(_itemUniqueId);
        }
    }

    public void SetSelected(bool isSelected)
    {
        if (_selectedImage == null)
        {
            return;
        }

        _selectedImage.gameObject.SetActive(isSelected);
    }
}
