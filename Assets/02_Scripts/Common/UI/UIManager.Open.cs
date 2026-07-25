using System;
using System.Collections.Generic;
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

    public void OpenQuestDetailUI(QuestModel questModel, int currentReputation, PetInventoryModel petInventoryModel)
    {
        if (questModel == null)
        {
            Debug.LogWarning("QuestModel이 null입니다.");
            return;
        }

        var view = OpenUI<QuestDetailUIView>(UIType.QuestDetailUIView);
        if (view == null)
        {
            Debug.LogWarning("QuestDetailUIView를 열 수 없습니다.");
            return;
        }

        view.SetCurrentReputation(currentReputation);
        view.SetAcceptedCallback(() => OpenPetInventoryUI(petInventoryModel));

        var viewModel = new QuestViewModel(questModel);
        view.BindViewModel(viewModel);
    }

    public void OpenMagicCircleUI(MagicCircleModel magicCircleModel)
    {
        if (magicCircleModel == null)
        {
            Debug.LogWarning("MagicCircleModel이 null입니다.");
            return;
        }

        var view = OpenUI<MagicCircleUIView>(UIType.MagicCircleUIView);
        if (view == null)
        {
            Debug.LogWarning("MagicCircleUIView를 열 수 없습니다.");
            return;
        }

        view.SetSoulSource(GameManager.Network.RequestPlayerOutGameViewModel());

        var viewModel = new MagicCircleViewModel(magicCircleModel);
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

    public ResourceHudUIView OpenResourceHudUI(ResourceModel resourceModel)
    {
        if (resourceModel == null)
        {
            Debug.LogWarning("ResourceModel이 null입니다.");
            return null;
        }

        var view = OpenUI<ResourceHudUIView>(UIType.ResourceHudUIView);
        if (view == null)
        {
            Debug.LogWarning("ResourceHudUIView를 열 수 없습니다.");
            return null;
        }

        var viewModel = new ResourceHudViewModel(resourceModel);
        view.BindViewModel(viewModel);

        view.SetSoulSource(GameManager.Network.RequestPlayerOutGameViewModel());
        return view;
    }

    public PartyHudUIView OpenPartyHudUI()
    {
        var view = OpenUI<PartyHudUIView>(UIType.PartyHudUIView);
        if (view == null)
        {
            Debug.LogWarning("PartyHudUIView를 열 수 없습니다.");
            return null;
        }
        return view;
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

    public void OpenSkillHudUI(string playerClassId)
    {
        var view = OpenUI<SkillHudUIView>(UIType.SkillHudUIView);
        if (view == null)
        {
            Debug.LogWarning("SkillHudUIView를 열 수 없습니다.");
            return;
        }

        view.SetSkills(playerClassId);
    }

    public void SetSkillHudUltimate(string skillId)
    {
        if (IsActiveUI(UIType.SkillHudUIView) == false)
        {
            return;
        }

        var view = GetCreatUI<SkillHudUIView>(UIType.SkillHudUIView);

        if (view == null)
        {
            return;
        }

        view.SetUltimateSkill(skillId);
    }

    public void OpenLevelUpUI(List<string> optionIdList, Action<string> onOptionSelected
        , Action onClosed = null)
    {
        var view = OpenUI<LevelUpUIView>(UIType.LevelUpUIView);

        if (view == null)
        {
            Debug.LogWarning("LevelUpUIView를 열 수 없습니다.");
            return;
        }

        view.SetOptions(optionIdList, onOptionSelected, onClosed);
    }

    public void OpenUltimateSelectUI(string playerClassId, Action<string> onSelected)
    {
        var view = OpenUI<UltimateSelectUIView>(UIType.UltimateSelectUIView);

        if (view == null)
        {
            Debug.LogWarning("UltimateSelectUIView를 열 수 없습니다.");
            return;
        }

        view.SetUltimates(playerClassId, onSelected);
    }

    public void OpenConvoyHudUI(WagonViewModel wagonViewModel, string questId)
    {
        var view = OpenUI<ConvoyHudUIView>(UIType.ConvoyHudUIView);

        if (view == null)
        {
            Debug.LogWarning("ConvoyHudUIView를 열 수 없습니다.");
            return;
        }
        
        view.SetConvoy(questId);
        view.BindViewModel(wagonViewModel);
    }

    public LoadingUIView OpenLoadingUI()
    {
        var view = OpenUI<LoadingUIView>(UIType.LoadingUIView);
        if (view == null)
        {
            Debug.LogWarning("LoadingUIView를 열 수 없습니다.");
            return null;
        }

        view.SetupRandom();
        return view;
    }

    public ScreenFadeUIView OpenScreenFade()
    {
        var view = OpenUI<ScreenFadeUIView>(UIType.ScreenFadeUIView);
        if (view == null)
        {
            Debug.LogWarning("ScreenFadeUIView를 열 수 없습니다.");
            return null;
        }

        return view;
    }

    public void OpenSimplePopup(string message)
    {
        var view = OpenUI<SimplePopupUIView>(UIType.SimplePopupUIView);
        if (view == null)
        {
            return;
        }

        view.SetPopup(message);
    }

    public void OpenPlayerHudUI(PlayerViewModel playerViewModel)
    {
        var view = OpenUI<PlayerHudUIView>(UIType.PlayerHudUIView);
        if (view == null)
        {
            Debug.LogWarning("PlayerHudUIView를 열 수 없습니다.");
            return;
        }

        view.BindViewModel(playerViewModel);
    }

    public void OpenConvoySuccessUI(ConvoyResultModel result)
    {
        if (result == null)
        {
            Debug.LogWarning("ConvoyResult가 null입니다.");
            return;
        }

        var view = OpenUI<ConvoySuccessUIView>(UIType.ConvoySuccessUIView);
        if (view == null)
        {
            Debug.LogWarning("ConvoySuccessUIView를 열 수 없습니다.");
            return;
        }

        view.SetResult(result);
    }

    public void OpenConvoyFailUI(ConvoyResultModel result)
    {
        if (result == null)
        {
            Debug.LogWarning("ConvoyResult가 null입니다.");
            return;
        }

        var view = OpenUI<ConvoyFailUIView>(UIType.ConvoyFailUIView);
        if (view == null)
        {
            Debug.LogWarning("ConvoyFailUIView를 열 수 없습니다.");
            return;
        }

        view.SetResult(result);
    }

    public void OpenWagonAreaWarningUI(WagonViewModel wagonViewModel)
    {
        if (wagonViewModel == null)
        {
            Debug.LogWarning("WagonViewModel이 null입니다.");
            return;
        }

        var view = OpenUI<WagonAreaWarningUIView>(UIType.WagonAreaWarningUIView);

        if (view == null)
        {
            Debug.LogWarning("WagonAreaWarningUIView를 열 수 없습니다.");
            return;
        }

        view.BindViewModel(wagonViewModel);
    }
    public void CloseAllConvoyHuds() 
    {
        CloseUI(UIType.ConvoyHudUIView);
        CloseUI(UIType.PartyHudUIView);
        CloseUI(UIType.PlayerHudUIView);
        CloseUI(UIType.ResourceHudUIView);
        CloseUI(UIType.SkillHudUIView);
        CloseUI(UIType.WagonAreaWarningUIView);
    }
}

