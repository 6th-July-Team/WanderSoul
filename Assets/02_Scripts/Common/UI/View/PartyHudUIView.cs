using System.Collections.Generic;
using UnityEngine;

public class PartyHudUIView : BaseUI
{
    [SerializeField] private PartyMemberSlotUIView _memberSlotPrefab;
    [SerializeField] private Transform _slotRoot;

    private PartyMemberSlotUIView _wagonSlot;
    private List<PartyMemberSlotUIView> _petSlotList = new();

    public void SetWagon(string name, float hpFillAmount)
    {
        if (_wagonSlot == null)
        {
            _wagonSlot = CreateSlot();
        }

        _wagonSlot.SetWagon(name, hpFillAmount);
    }

    public void AddPet(string name, float hpFillAmount)
    {
        var slot = CreateSlot();
        slot.SetPet(name, hpFillAmount);
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
