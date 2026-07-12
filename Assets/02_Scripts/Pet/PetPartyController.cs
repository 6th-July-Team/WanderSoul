using System.Collections.Generic;
using UnityEngine;

public class PetPartyController : IPetPartyReader
{
    private List<PetController> _pets = new();

    private PlayerMovement _player;
    private Wagon _wagon;

    public int Count => _pets.Count;

    public void Init(PlayerMovement player, Wagon wagon, List<PetController> selectedPets)
    {
        _player = player;
        _wagon = wagon;

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

    // TODO(김익환): 편성된 펫 속성 비중 계산
    //    public EResolvedElement GetCurrentElement()
    //    {
    //        return EResolvedElement.Magic;
    //    }
}

// 이거 고민
//public enum EResolvedElement
//{
//    Fire,
//    Water,
//    Earth,
//    Air,
//    Magic
//}