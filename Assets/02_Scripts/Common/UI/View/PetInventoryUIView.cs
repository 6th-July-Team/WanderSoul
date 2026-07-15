using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PetInventoryUIView : BaseUI<PetInventoryUIView, PetInventoryViewModel>
{
    #region Variables

    [Header("Inventory")]
    [SerializeField] private GameObject _petSlotPrefab;
    [SerializeField] private Transform _slotRoot;
    [SerializeField] private UIButton _closeButton;

    [Header("Category")]
    [SerializeField] private UIButton _allCategoryButton;
    [SerializeField] private UIButton _fireCategoryButton;
    [SerializeField] private UIButton _waterCategoryButton;
    [SerializeField] private UIButton _earthCategoryButton;
    [SerializeField] private UIButton _airCategoryButton;

    [Header("Party")]
    [SerializeField] private PartySlotUIView[] _partySlotArray;
    [SerializeField] private TMP_Text _partyCountText;
    [SerializeField] private UIButton _startMissionButton;

    [Header("Tooltip")]
    [SerializeField] private PetTooltipUIView _petTooltip;

    private List<PetSlotUIView> _slotList = new List<PetSlotUIView>();

    #endregion

    protected override void OnInit()
    {
        _closeButton.BindOnClickButtonEvent(OnClickClose);
        _startMissionButton.BindOnClickButtonEvent(OnClickStartMission);

        BindCategoryButtons();
        BindPartySlots();
    }

    private void BindCategoryButtons()
    {
        _allCategoryButton.BindOnClickButtonEvent(OnClickAllCategory);
        _fireCategoryButton.BindOnClickButtonEvent(OnClickFireCategory);
        _waterCategoryButton.BindOnClickButtonEvent(OnClickWaterCategory);
        _earthCategoryButton.BindOnClickButtonEvent(OnClickEarthCategory);
        _airCategoryButton.BindOnClickButtonEvent(OnClickAirCategory);
    }

    private void BindPartySlots()
    {
        for (int i = 0; i < _partySlotArray.Length; i++)
        {
            _partySlotArray[i].InitSlot(i);
            _partySlotArray[i].BindSlotClickEvent(OnClickPartySlot);
        }
    }

    private void OnEnable()
    {
        _petTooltip.HideTooltip();
    }

    protected override void OnPropertyChanged(string propertyName)
    {
        if (propertyName == nameof(PetInventoryModel.PetList))
        {
            RefreshSlots();
            RefreshPartySlots();
            return;
        }

        if (propertyName == nameof(PartyModel.PartyPetIdList))
        {
            RefreshPartySlots();
            RefreshSlotPartyState();
            return;
        }
    }

    #region Category
    private void OnClickAllCategory()
    {
        _viewModel.ChangeFilterElement(string.Empty);
    }

    private void OnClickFireCategory()
    {
        _viewModel.ChangeFilterElement("Fire");
    }

    private void OnClickWaterCategory()
    {
        _viewModel.ChangeFilterElement("Water");
    }

    private void OnClickEarthCategory()
    {
        _viewModel.ChangeFilterElement("Earth");
    }

    private void OnClickAirCategory()
    {
        _viewModel.ChangeFilterElement("Air");
    }

    #endregion

    #region Inventory Slots

    private void RefreshSlots()
    {
        ClearSlots();

        var petList = _viewModel.GetFilteredPetList();
        if (petList == null)
        {
            return;
        }

        foreach (var pet in petList)
        {
            CreateSlot(pet);
        }

        RefreshSlotPartyState();
    }

    private void CreateSlot(PetSlotModel pet)
    {
        var slotObj = Instantiate(_petSlotPrefab, _slotRoot);
        if (slotObj == null)
        {
            return;
        }

        var slot = slotObj.GetComponent<PetSlotUIView>();
        if (slot == null)
        {
            return;
        }

        slot.InitSlot(pet);
        slot.BindSlotEvent(OnClickPetSlot, OnHoverEnterPetSlot, OnHoverExitPetSlot);
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

    private void RefreshSlotPartyState()
    {
        foreach (var slot in _slotList)
        {
            bool isInParty = _viewModel.IsInParty(slot.PetUniqueId);
            slot.SetInPartyState(isInParty);
        }
    }

    private void OnClickPetSlot(long petUniqueId)
    {
        _viewModel.TogglePartyMember(petUniqueId);
    }

    private void OnHoverEnterPetSlot(long petUniqueId, PetSlotUIView slot)
    {
        var pet = _viewModel.GetPet(petUniqueId);
        if (pet == null)
        {
            return;
        }

        _petTooltip.ShowTooltip(pet);
    }

    private void OnHoverExitPetSlot()
    {
        _petTooltip.HideTooltip();
    }

    #endregion

    #region Party

    private void RefreshPartySlots()
    {
        for (int i = 0; i < _partySlotArray.Length; i++)
        {
            long petUniqueId = _viewModel.GetPartyPetIdBySlotIndex(i);

            if (petUniqueId == 0)
            {
                _partySlotArray[i].SetEmpty();
                continue;
            }

            var pet = _viewModel.GetPet(petUniqueId);
            _partySlotArray[i].SetPet(pet);
        }

        RefreshPartyCountText();
    }

    private void RefreshPartyCountText()
    {
        _partyCountText.text = $"{_viewModel.PartyCount} / {_viewModel.MaxPartyCount}";
    }

    private void OnClickPartySlot(int slotIndex)
    {
        long petUniqueId = _viewModel.GetPartyPetIdBySlotIndex(slotIndex);
        if (petUniqueId == 0)
        {
            return;
        }

        _viewModel.TogglePartyMember(petUniqueId);
    }

    private void OnClickStartMission()
    {
        if (_viewModel.CanStartMission() == false)
        {
            Debug.LogWarning("파티에 펫이 한 마리도 없습니다.");
            return;
        }

        // TODO(이태영): 스테이지 진입
    }

    #endregion

    private void OnClickClose()
    {
        GameManager.UI.CloseUI(UIType.PetInventoryUIView);
    }
}