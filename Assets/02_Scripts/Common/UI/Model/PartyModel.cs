using System.Collections.Generic;

public class PartyModel : BaseModel
{
    private const int MAX_PARTY_COUNT = 3;
    private List<long> _partyPetIdList = new List<long>();

    public List<long> PartyPetIdList { get { return _partyPetIdList; } }
    public int MaxPartyCount { get { return MAX_PARTY_COUNT; } }
    public int PartyCount { get { return _partyPetIdList.Count; } }
    public bool IsPartyFull { get { return PartyCount >= MAX_PARTY_COUNT; } }

    public bool IsInParty(long petUniqueId)
    {
        return _partyPetIdList.Contains(petUniqueId);
    }

    public long GetPartyPetIdBySlotIndex(int slotIndex)
    {
        if (slotIndex < 0 || slotIndex >= _partyPetIdList.Count)
        {
            return 0;
        }

        return _partyPetIdList[slotIndex];
    }

    public bool TogglePartyMember(long petUniqueId)
    {
        if (IsInParty(petUniqueId) == true)
        {
            _partyPetIdList.Remove(petUniqueId);
            OnPropertyChanged(nameof(PartyPetIdList));
            return true;
        }

        if (IsPartyFull == true)
        {
            return false;
        }

        _partyPetIdList.Add(petUniqueId);
        OnPropertyChanged(nameof(PartyPetIdList));
        return true;
    }

    public void ClearParty()
    {
        _partyPetIdList.Clear();
        OnPropertyChanged(nameof(PartyPetIdList));
    }

    public override void PropertyChangedOnInit()
    {
        OnPropertyChanged(nameof(PartyPetIdList));
    }
}
