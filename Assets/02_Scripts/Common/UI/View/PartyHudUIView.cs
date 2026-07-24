using System.Collections.Generic;
using UnityEngine;

public class PartyHudUIView : BaseUI
{
    [SerializeField] private PartyMemberSlotUIView _memberSlotPrefab;
    [SerializeField] private Transform _slotRoot;
    [SerializeField] private UISlideAnimation _slideAnimation;

    private PartyMemberSlotUIView _wagonSlot;
    private List<PartyMemberSlotUIView> _petSlotList = new();

    protected override void OnOpened()
    {
        if (_slideAnimation == null)
        {
            return;
        }

        _slideAnimation.SetHidden();
        _slideAnimation.SlideIn();
    }

    public void SetWagon(string name, float hpFillAmount)
    {
        if (_wagonSlot == null)
        {
            _wagonSlot = CreateSlot();
        }

        _wagonSlot.SetWagon(name, hpFillAmount);
    }

    public void AddPet(string petId, float hpFillAmount)
    {
        var slot = CreateSlot();
        slot.SetPet(petId, hpFillAmount);
        _petSlotList.Add(slot);
    }

    public void RefreshWagonHp(float hpFillAmount)
    {
        if (_wagonSlot != null)
        {
            _wagonSlot.RefreshHp(hpFillAmount);
        }
    }

    public void RefreshPetHp(int index, float hpFillAmount)
    {
        if (index < 0 || index >= _petSlotList.Count)
        {
            return;
        }

        _petSlotList[index].RefreshHp(hpFillAmount);
    }

    public void ClearPets()
    {
        foreach (var slot in _petSlotList)
        {
            if (slot != null)
            {
                Destroy(slot.gameObject);
            }
        }

        _petSlotList.Clear();
    }

    private PartyMemberSlotUIView CreateSlot()
    {
        return Instantiate(_memberSlotPrefab, _slotRoot);
    }
}
