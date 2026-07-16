using UnityEngine;

public partial class UIManager
{
    public T OpenUI<T>(UIType uiType) where T : BaseUI
    {
        T ui = GetCreatUI<T>(uiType);

        if(ui == null)
        {
            return null;
        }

        if (_activeUI.Contains(uiType) == false)
        {
            ui.ActiveTrue();
            _activeUI.Add(uiType);
        }
        return ui;
    }

    public void CloseUI(UIType uiType)
    {

        if (_activeUI.Contains(uiType) == false)
        {
            return;
        }
   
        if (_createdUIDic.TryGetValue(uiType, out BaseUI ui) == false)
        {
            _activeUI.Remove(uiType);
            return;
        }

        ui.ActiveFalse();
        _activeUI.Remove(uiType);
    }

    public bool IsActiveUI(UIType uiType)
    {
        return _activeUI.Contains(uiType);
    }

    public void OpenPetInventoryUI(PetInventoryModel petInventoryModel)
    {
        if (petInventoryModel == null)
        {
            Debug.LogWarning("PetInventoryModel이 null입니다.");
            return;
        }

        var view = OpenUI<PetInventoryUIView>(UIType.PetInventoryUIView);
        if (view == null)
        {
            Debug.LogWarning("PetInventoryUIView를 열 수 없습니다.");
            return;
        }

        var partyModel = new PartyModel();
        var viewModel = new PetInventoryViewModel(petInventoryModel, partyModel);

        view.BindViewModel(viewModel);
    }

    public void OpenInventoryUI(InventoryModel inventoryModel)
    {
        if (inventoryModel == null)
        {
            Debug.LogWarning("InventoryModel이 null입니다.");
            return;
        }

        var view = OpenUI<InventoryUIView>(UIType.InventoryUIView);
        if (view == null)
        {
            Debug.LogWarning("InventoryUIView를 열 수 없습니다.");
            return;
        }

        var viewModel = new InventoryViewModel(inventoryModel);
        view.BindViewModel(viewModel);
    }

    public void OpenResourceHudUI(ResourceModel resourceModel)
    {
        if (resourceModel == null)
        {
            Debug.LogWarning("ResourceModel이 null입니다.");
            return;
        }

        var view = OpenUI<ResourceHudUIView>(UIType.ResourceHudUIView);
        if (view == null)
        {
            Debug.LogWarning("ResourceHudUIView를 열 수 없습니다.");
            return;
        }

        var viewModel = new ResourceHudViewModel(resourceModel);
        view.BindViewModel(viewModel);
    }

    public void OpenPartyHudUI(PartyMemberModel partyMemberModel)
    {
        if (partyMemberModel == null)
        {
            Debug.LogWarning("PartyMemberModel이 null입니다.");
            return;
        }

        var view = OpenUI<PartyHudUIView>(UIType.PartyHudUIView);
        if (view == null)
        {
            Debug.LogWarning("PartyHudUIView를 열 수 없습니다.");
            return;
        }

        var viewModel = new PartyHudViewModel(partyMemberModel);
        view.BindViewModel(viewModel);
    }

    public void OpenVillageInfoHudUI(VillageModel villageModel)
    {
        if (villageModel == null)
        {
            Debug.LogWarning("VillageModel이 null입니다.");
            return;
        }

        var view = OpenUI<VillageInfoHudUIView>(UIType.VillageInfoHudUIView);
        if (view == null)
        {
            Debug.LogWarning("VillageInfoHudUIView를 열 수 없습니다.");
            return;
        }

        var viewModel = new VillageInfoViewModel(villageModel);
        view.BindViewModel(viewModel);
    }

    public void OpenSkillHudUI()
    {
        var view = OpenUI<SkillHudUIView>(UIType.SkillHudUIView);
        if (view == null)
        {
            Debug.LogWarning("SkillHudUIView를 열 수 없습니다.");
        }
    }

    public void OpenSkillHudUI(PlayerCombatController combatController)
    {
        var view = OpenUI<SkillHudUIView>(UIType.SkillHudUIView);
        if (view == null)
        {
            Debug.LogWarning("SkillHudUIView를 열 수 없습니다.");
            return;
        }

        view.SetSkills(combatController);
    }

}

