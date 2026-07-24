using System;
using System.Collections.Generic;
using UnityEngine;

public class PartyHudUIView : BaseUI
{
    [SerializeField] private PartyMemberSlotUIView _memberSlotPrefab;
    [SerializeField] private Transform _slotRoot;
    [SerializeField] private UISlideAnimation _slideAnimation;

    private PartyMemberSlotUIView _wagonSlot;
    private WagonViewModel _wagonViewModel;
    private Action<string> _wagonHandler;

    private List<PartyMemberSlotUIView> _petSlotList = new();
    private List<PetViewModel> _petViewModels = new();
    private List<Action<string>> _petHandlers = new();

    protected override void OnOpened()
    {
        if (_slideAnimation == null)
        {
            return;
        }

        _slideAnimation.SetHidden();
        _slideAnimation.SlideIn();
    }

    protected override void OnClosed()
    {
        ClearPets();
        UnbindWagon();
    }

    public void BindWagon(WagonViewModel wagonViewModel)
    {
        UnbindWagon();

        if (wagonViewModel == null)
        {
            return;
        }

        if (_wagonSlot == null)
        {
            _wagonSlot = CreateSlot();
        }

        var slot = _wagonSlot;

        float maxDurability = wagonViewModel.GetDurability;

        slot.SetWagon(wagonViewModel.GetWagonName
            , GetFillAmount(wagonViewModel.GetDurability, maxDurability));

        _wagonViewModel = wagonViewModel;
        _wagonHandler = (propertyName) => OnWagonPropertyChanged(propertyName, slot, wagonViewModel, maxDurability);
        wagonViewModel.OnPropertyChanged_View += _wagonHandler;
    }

    public void BindPet(string petId, PetViewModel petViewModel)
    {
        if (petViewModel == null)
        {
            return;
        }

        var slot = CreateSlot();
        slot.SetPet(petId, GetFillAmount(petViewModel.GetHp, petViewModel.GetMaxHp));

        Action<string> handler = (propertyName) => OnPetPropertyChanged(propertyName, slot, petViewModel);
        petViewModel.OnPropertyChanged_View += handler;

        _petSlotList.Add(slot);
        _petViewModels.Add(petViewModel);
        _petHandlers.Add(handler);
    }

    private void OnWagonPropertyChanged(string propertyName, PartyMemberSlotUIView slot, WagonViewModel wagonViewModel, float maxDurability)
    {
        if (propertyName == nameof(WagonModel.Durability))
        {
            slot.RefreshHp(GetFillAmount(wagonViewModel.GetDurability, maxDurability));
        }
    }

    private void OnPetPropertyChanged(string propertyName, PartyMemberSlotUIView slot, PetViewModel petViewModel)
    {
        if (propertyName == nameof(PetModel.HP))
        {
            slot.RefreshHp(GetFillAmount(petViewModel.GetHp, petViewModel.GetMaxHp));
        }
    }

    public void ClearPets()
    {

        for (int i = 0; i < _petViewModels.Count; i++)
        {
            _petViewModels[i].OnPropertyChanged_View -= _petHandlers[i];
        }

        _petViewModels.Clear();
        _petHandlers.Clear();

        foreach (var slot in _petSlotList)
        {
            if (slot != null)
            {
                Destroy(slot.gameObject);
            }
        }

        _petSlotList.Clear();
    }

    private void UnbindWagon()
    {
        if (_wagonViewModel == null)
        {
            return;
        }

        _wagonViewModel.OnPropertyChanged_View -= _wagonHandler;
        _wagonViewModel = null;
        _wagonHandler = null;
    }

    private float GetFillAmount(float current, float max)
    {
        if (max <= 0f)
        {
            return 0f;
        }

        return current / max;
    }

    private PartyMemberSlotUIView CreateSlot()
    {
        return Instantiate(_memberSlotPrefab, _slotRoot);
    }
}
