using System;
using UnityEngine;
using UnityEngine.UI;

public class PartySlotUIView : MonoBehaviour
{
    [SerializeField] private Image _iconImage;
    [SerializeField] private UIButton _slotButton;

    private int _slotIndex;

    private event Action<int> OnSlotClicked;

    public int SlotIndex
    {
        get { return _slotIndex; }
    }

    private void OnEnable()
    {
        _slotButton.BindOnClickButtonEvent(OnClickSlot);
    }

    private void OnDisable()
    {
        OnSlotClicked = null;
    }

    public void InitSlot(int slotIndex)
    {
        _slotIndex = slotIndex;
        SetEmpty();
    }

    public void BindSlotClickEvent(Action<int> onClicked)
    {
        OnSlotClicked = onClicked;
    }

    public void SetPet(PetSlotModel pet)
    {
        if (pet == null)
        {
            SetEmpty();
            return;
        }

        var petData = GameManager.DataTable.GetPetData(pet.PetDataId);
        if (petData == null)
        {
            SetEmpty();
            return;
        }

        RefreshIcon(petData.IconPath);
    }

    public void SetEmpty()
    {
        _iconImage.enabled = false;
    }

    private void RefreshIcon(string iconPath)
    {
        if (string.IsNullOrEmpty(iconPath) == true)
        {
            SetEmpty();
            return;
        }

        Sprite iconSprite = Utils.ResourcesLoad<Sprite>(iconPath);
        if (iconSprite == null)
        {
            SetEmpty();
            return;
        }

        _iconImage.sprite = iconSprite;
        _iconImage.enabled = true;
    }

    private void OnClickSlot()
    {
        if (OnSlotClicked != null)
        {
            OnSlotClicked.Invoke(_slotIndex);
        }
    }
}