using System.Collections.Generic;

public class PetPartyController : IPetPartyReader
{
    private List<PetController> _pets = new();

    public int Count => _pets.Count;

    public void Init(List<PetController> selectedPets)
    {
        RegisterPet(selectedPets);

        SetPetCommand(PetCommand.PlayerFollow);
    }

    private void RegisterPet(List<PetController> selectedPets)
    {
        _pets.Clear();

        for (int i = 0; i < selectedPets.Count; i++)
        {
            _pets.Add(selectedPets[i]);
        }
    }

    public void UnregisterPet()
    {
        _pets.Clear();
    }

    public void SetPetCommand(PetCommand commandMode)
    {
        foreach (var pet in _pets)
        {
            pet.SetCommandMode(commandMode);
        }
    }

    public PetElement GetPriorityPetElement()
    {
        int elementMask = 0;

        foreach (PetController pet in _pets)
        {
            int bitPosition = 1 << (int)pet.Element;

            if ((elementMask & bitPosition) != 0)
                return pet.Element;

            elementMask |= bitPosition;
        }

        return PetElement.None;
    }

    public void AddModifierForAllPet(PetStatModifier modifier)
    {
        foreach(var pet in _pets)
        {
            pet.AddModifier(modifier);
        }
    }

    public void RemoveModifierForAllPet(PetStatType petStatType)
    {
        foreach (var pet in _pets)
        {
            pet.RemoveModifiers(petStatType);
        }
    }
}