using System.Collections.Generic;
using System.ComponentModel;

public class PetInventoryViewModel : BaseViewModel
{
    private readonly PetInventoryModel _petInventoryModel;
    private readonly PartyModel _partyModel;

    private string _currentFilterElement = string.Empty;

    public List<PetSlotModel> PetList
    {
        get { return _petInventoryModel.PetList; }
    }

    public int PartyCount
    {
        get { return _partyModel.PartyCount; }
    }

    public int MaxPartyCount
    {
        get { return _partyModel.MaxPartyCount; }
    }

    public string CurrentFilterElement
    {
        get { return _currentFilterElement; }
    }

    public PetInventoryViewModel(PetInventoryModel petInventoryModel, PartyModel partyModel)
    {
        _petInventoryModel = petInventoryModel;
        _partyModel = partyModel;

        _petInventoryModel.PropertyChanged += OnPropertyChanged;
        _partyModel.PropertyChanged += OnPropertyChanged;
    }

    public override void Dispose()
    {
        _petInventoryModel.PropertyChanged -= OnPropertyChanged;
        _partyModel.PropertyChanged -= OnPropertyChanged;
    }

    public override void PropertyChangedOnInit()
    {
        _petInventoryModel.PropertyChangedOnInit();
        _partyModel.PropertyChangedOnInit();
    }

    public List<PetSlotModel> GetFilteredPetList()
    {
        var result = new List<PetSlotModel>();

        foreach (var pet in _petInventoryModel.PetList)
        {
            if (IsMatchFilter(pet) == true)
            {
                result.Add(pet);
            }
        }

        return result;
    }

    private bool IsMatchFilter(PetSlotModel pet)
    {
        if (string.IsNullOrEmpty(_currentFilterElement) == true)
        {
            return true;
        }

        var petData = GameManager.DataTable.GetPetData(pet.PetDataId);
        if (petData == null)
        {
            return false;
        }

        return petData.ElementType == _currentFilterElement;
    }

    public void ChangeFilterElement(string elementType)
    {
        if (_currentFilterElement == elementType)
        {
            return;
        }

        _currentFilterElement = elementType;
        _petInventoryModel.PropertyChangedOnInit();
    }

    public bool IsInParty(long petUniqueId)
    {
        return _partyModel.IsInParty(petUniqueId);
    }

    public long GetPartyPetIdBySlotIndex(int slotIndex)
    {
        return _partyModel.GetPartyPetIdBySlotIndex(slotIndex);
    }

    public PetSlotModel GetPet(long petUniqueId)
    {
        return _petInventoryModel.GetPet(petUniqueId);
    }

    public bool TogglePartyMember(long petUniqueId)
    {
        return _partyModel.TogglePartyMember(petUniqueId);
    }

    public bool CanStartMission()
    {
        return _partyModel.PartyCount > 0;
    }
}