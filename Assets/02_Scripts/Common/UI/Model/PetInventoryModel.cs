using System;
using System.Collections.Generic;

[Serializable]
public class PetInventoryModel : BaseModel
{
    private List<PetSlotModel> _petList = new List<PetSlotModel>();

    [NonSerialized]
    private Dictionary<long, PetSlotModel> _petDic = new Dictionary<long, PetSlotModel>();

    public List<PetSlotModel> PetList
    {
        get { return _petList; }
    }

    public void AddPet(PetSlotModel pet)
    {
        if (pet == null)
        {
            return;
        }

        _petList.Add(pet);
        _petDic[pet.PetUniqueId] = pet;
        OnPropertyChanged(nameof(PetList));
    }

    public void RemovePet(long petUniqueId)
    {
        if (_petDic.ContainsKey(petUniqueId) == false)
        {
            return;
        }

        var pet = _petDic[petUniqueId];
        _petList.Remove(pet);
        _petDic.Remove(petUniqueId);
        OnPropertyChanged(nameof(PetList));
    }

    public PetSlotModel GetPet(long petUniqueId)
    {
        if (_petDic.ContainsKey(petUniqueId) == false)
        {
            return null;
        }

        return _petDic[petUniqueId];
    }

    public void RebuildDictionary()
    {
        _petDic.Clear();

        foreach (var pet in _petList)
        {
            _petDic[pet.PetUniqueId] = pet;
        }
    }

    public override void PropertyChangedOnInit()
    {
        OnPropertyChanged(nameof(PetList));
    }
}